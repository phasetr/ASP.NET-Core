using System.IdentityModel.Tokens.Jwt;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace AuthFunction;

public class Function
{
    private readonly DynamoDBContext _context;

    /// <summary>
    ///     Default constructor to read environment variables, Get the JWKs, and initialize DynamoDB context
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public Function()
    {
        LambdaLogger.Log("Initiating the default values");
        var envRegion = Environment.GetEnvironmentVariable("REGION");
        var envCognitoUserPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID");
        var envClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        if (string.IsNullOrEmpty(envRegion) || string.IsNullOrEmpty(envCognitoUserPoolId) ||
            string.IsNullOrEmpty(envClientId))
            throw new ArgumentNullException("REGION or COGNITO_USER_POOL_ID or CLIENT_ID");

        ClientId = envClientId;
        UserPool = $"https://cognito-idp.{envRegion}.amazonaws.com/{envCognitoUserPoolId}";
        var keyUrl = UserPool + "/.well-known/jwks.json";
        Jwks = GetJwks(keyUrl).Result;

        var dynamoDbClient = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(dynamoDbClient);
    }

    private string Jwks { get; }
    private string ClientId { get; }
    private string UserPool { get; }

    /// <summary>
    ///     Method to make GET JWKs by calling Cognito User pool Key URL
    /// </summary>
    /// <param name="keyUrl"></param>
    /// <returns>Task</returns>
    private static async Task<string> GetJwks(string keyUrl)
    {
        var client = new HttpClient();
        return await client.GetStringAsync(keyUrl).ConfigureAwait(false);
    }

    /// <summary>
    ///     Lambda function handler to validate JWT token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>APIGatewayCustomAuthorizerResponse</returns>
    public APIGatewayCustomAuthorizerResponse FunctionHandler(
        APIGatewayCustomAuthorizerRequest request,
        ILambdaContext context)
    {
        LambdaLogger.Log("Received Auth request");
        /* Validating JWT token in three steps as documented at - https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html
          Step 1: Confirm the structure of the JWT
          Step 2: Validate the JWT signature
          Step 3: Verify the claims
        */

        var token = GetTokenFromRequest(request.AuthorizationToken);

        // Step 1: Confirm the structure of the JWT
        if (!IsValidJwtStructure(token)) throw new Exception("Unauthorized");

        // Step 2: Validate the JWT signature
        var jwtSecurityToken = ValidateJwtSignature(token);
        if (jwtSecurityToken == null) throw new Exception("Unauthorized");

        // Step 3: Verify the claims
        var userGroup = VerifyClaims(jwtSecurityToken);
        if (string.IsNullOrEmpty(userGroup)) throw new Exception("Unauthorized");

        // Get policy document based on user group
        var policyDocument = GetApiGwAccessPolicy(userGroup);

        if (string.IsNullOrEmpty(policyDocument))
            // Return deny policy
            return new APIGatewayCustomAuthorizerResponse
            {
                PrincipalID = "yyyyyyyy",
                PolicyDocument = JsonConvert.DeserializeObject<APIGatewayCustomAuthorizerPolicy>(GetDenyPolicy()),
                UsageIdentifierKey = ""
            };

        return new APIGatewayCustomAuthorizerResponse
        {
            // Return access policy
            PrincipalID = "yyyyyyyy",
            PolicyDocument = JsonConvert.DeserializeObject<APIGatewayCustomAuthorizerPolicy>(policyDocument),
            UsageIdentifierKey = ""
        };
    }

    /// <summary>
    ///     Get token from request
    /// </summary>
    /// <param name="authorizationHeader"></param>
    /// <returns>string</returns>
    private static string GetTokenFromRequest(string authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader)) return string.Empty;
        var authHeaders = authorizationHeader.Split(" ");
        LambdaLogger.Log("authHearers.Length: " + authHeaders.Length);
        if (authHeaders.Length == 2 && authHeaders[0] == "Bearer") return authHeaders[1];
        return string.Empty;
    }

    /// <summary>
    ///     Validate JWT structure
    /// </summary>
    /// <param name="token"></param>
    /// <returns>bool</returns>
    private static bool IsValidJwtStructure(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        return token.Split(".").Length == 3;
    }

    /// <summary>
    ///     Verify JWT claims and return user group
    /// </summary>
    /// <param name="jwtSecurityToken"></param>
    /// <returns>string</returns>
    private string VerifyClaims(JwtSecurityToken? jwtSecurityToken)
    {
        if (jwtSecurityToken == null) return string.Empty;
        // Note: Token expiration already verified in ValidateJwtSignature method.  
        try
        {
            var clientId = jwtSecurityToken.Claims.First(x => x.Type == "client_id").Value;
            if (clientId != ClientId) return string.Empty;

            var iss = jwtSecurityToken.Claims.First(x => x.Type == "iss").Value;
            if (iss != UserPool) return string.Empty;

            var tokenUse = jwtSecurityToken.Claims.First(x => x.Type == "token_use").Value;
            return tokenUse != "access"
                ? string.Empty
                : jwtSecurityToken.Claims.First(x => x.Type == "cognito:groups").Value;
        }
        catch (Exception)
        {
            // Exception when claim is missing
            return string.Empty;
        }
    }

    /// <summary>
    ///     Validate JWT signature
    /// </summary>
    /// <param name="token"></param>
    /// <returns>JwtSecurityToken</returns>
    private JwtSecurityToken? ValidateJwtSignature(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKeys = new JsonWebKeySet(Jwks).GetSigningKeys();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKeys = signingKeys,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero //set expiration time same as JWT expiration time
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            return jwtToken;
        }
        catch (Exception)
        {
            // return null if JWT validation fails
            return null;
        }
    }

    /// <summary>
    ///     Get API GW access policy document
    /// </summary>
    /// <param name="userGroup"></param>
    /// <returns>string</returns>
    private string GetApiGwAccessPolicy(string userGroup)
    {
        var data = _context.LoadAsync<DynamoDbTableModel>(userGroup).Result;
        return data != null ? data.ApiGwAccessPolicy : string.Empty;
    }

    /// <summary>
    ///     Get API GW deny policy document
    /// </summary>
    /// <returns>string</returns>
    private static string GetDenyPolicy()
    {
        return
            "{\"Version\": \"2012-10-17\",\"Statement\": [ {\"Effect\": \"Deny\",\"Principal\": \"*\", \"Action\": [\"execute-api:Invoke\"], \"Resource\": [ \"arn:aws:execute-api:*:*:*\"]}]}";
    }
}