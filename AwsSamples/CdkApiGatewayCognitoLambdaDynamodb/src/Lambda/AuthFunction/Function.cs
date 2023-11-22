using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
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
        if (string.IsNullOrEmpty(envRegion) ||
            string.IsNullOrEmpty(envCognitoUserPoolId) ||
            string.IsNullOrEmpty(envClientId))
            throw new ArgumentNullException("REGION or COGNITO_USER_POOL_ID or CLIENT_ID");

        ClientId = envClientId;
        UserPool = $"https://cognito-idp.{envRegion}.amazonaws.com/{envCognitoUserPoolId}";
        var keyUrl = UserPool + "/.well-known/jwks.json";
        Jwks = Helpers.GetJwks(keyUrl).Result;

        var dynamoDbClient = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(dynamoDbClient);
    }

    private string Jwks { get; }
    private string ClientId { get; }
    private string UserPool { get; }

    /// <summary>
    ///     Lambda function handler to validate JWT token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>APIGatewayCustomAuthorizerResponse</returns>
    public async Task<APIGatewayCustomAuthorizerResponse> FunctionHandler(
        APIGatewayCustomAuthorizerRequest request,
        ILambdaContext context)
    {
        LambdaLogger.Log("Received Auth request");

        /*
         Validating JWT token in three steps as documented at - https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html
         Step 1: Confirm the structure of the JWT
         Step 2: Validate the JWT signature
         Step 3: Verify the claims
        */

        var token = Helpers.GetTokenFromRequest(request.AuthorizationToken);

        // Step 1: Confirm the structure of the JWT
        if (!Helpers.IsValidJwtStructure(token)) throw new Exception("Unauthorized: invalid token structure");

        // Step 2: Validate the JWT signature
        var jwtSecurityToken = Helpers.ValidateJwtSignature(Jwks, token);
        if (jwtSecurityToken == null) throw new Exception("Unauthorized: invalid token signature");

        // Step 3: Verify the claims
        var userGroup = Helpers.VerifyClaims(ClientId, jwtSecurityToken, UserPool);
        if (string.IsNullOrEmpty(userGroup)) throw new Exception("Unauthorized: invalid token claims");

        // Get policy document based on user group
        var policyDocument = await GetApiGwAccessPolicy(userGroup);

        if (string.IsNullOrEmpty(policyDocument))
            // Return deny policy
            return new APIGatewayCustomAuthorizerResponse
            {
                PrincipalID = "yyyyyyyy",
                PolicyDocument =
                    JsonConvert.DeserializeObject<APIGatewayCustomAuthorizerPolicy>(Helpers.GetDenyPolicy()),
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
    ///     Get API GW access policy document
    /// </summary>
    /// <param name="userGroup"></param>
    /// <returns>string</returns>
    private async Task<string> GetApiGwAccessPolicy(string userGroup)
    {
        var data = await _context.LoadAsync<DynamoDbTableModel>(userGroup);
        return data != null ? data.ApiGwAccessPolicy : string.Empty;
    }
}