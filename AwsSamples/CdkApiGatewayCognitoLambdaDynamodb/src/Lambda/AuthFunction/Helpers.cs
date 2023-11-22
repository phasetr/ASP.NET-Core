using System.IdentityModel.Tokens.Jwt;
using Amazon.Lambda.Core;
using Microsoft.IdentityModel.Tokens;

namespace AuthFunction;

public class Helpers
{
    /// <summary>
    ///     Get API GW deny policy document
    /// </summary>
    /// <returns>string</returns>
    public static string GetDenyPolicy()
    {
        return
            "{\"Version\": \"2012-10-17\",\"Statement\": [ {\"Effect\": \"Deny\",\"Principal\": \"*\", \"Action\": [\"execute-api:Invoke\"], \"Resource\": [ \"arn:aws:execute-api:*:*:*\"]}]}";
    }

    /// <summary>
    ///     Method to make GET JWKs by calling Cognito User pool Key URL
    /// </summary>
    /// <param name="keyUrl"></param>
    /// <returns>Task</returns>
    public static async Task<string> GetJwks(string keyUrl)
    {
        var client = new HttpClient();
        return await client.GetStringAsync(keyUrl).ConfigureAwait(false);
    }

    /// <summary>
    ///     Get token from request
    /// </summary>
    /// <param name="authorizationHeader"></param>
    /// <returns>string</returns>
    public static string GetTokenFromRequest(string authorizationHeader)
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
    public static bool IsValidJwtStructure(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        return token.Split(".").Length == 3;
    }

    /// <summary>
    ///     Validate JWT signature
    /// </summary>
    /// <param name="jwks"></param>
    /// <param name="token"></param>
    /// <returns>JwtSecurityToken</returns>
    public static JwtSecurityToken? ValidateJwtSignature(string jwks, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKeys = new JsonWebKeySet(jwks).GetSigningKeys();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKeys = signingKeys,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // set expiration time same as JWT expiration time
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            return jwtToken;
        }
        catch (Exception e)
        {
            LambdaLogger.Log("Exception: " + e.Message);
            // return null if JWT validation fails
            return null;
        }
    }

    /// <summary>
    ///     Verify JWT claims and return user group
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="jwtSecurityToken"></param>
    /// <param name="userPool"></param>
    /// <returns>string</returns>
    public static string VerifyClaims(
        string clientId,
        JwtSecurityToken? jwtSecurityToken,
        string userPool)
    {
        if (jwtSecurityToken == null) return string.Empty;
        // Note: Token expiration already verified in ValidateJwtSignature method.  
        try
        {
            var clientIdFromToken = jwtSecurityToken.Claims.First(x => x.Type == "client_id").Value;
            if (clientIdFromToken != clientId) return string.Empty;

            var iss = jwtSecurityToken.Claims.First(x => x.Type == "iss").Value;
            if (iss != userPool) return string.Empty;

            var tokenUse = jwtSecurityToken.Claims.First(x => x.Type == "token_use").Value;
            return tokenUse != "access"
                ? string.Empty
                : jwtSecurityToken.Claims.First(x => x.Type == "cognito:groups").Value;
        }
        catch (Exception e)
        {
            LambdaLogger.Log("Exception: " + e.Message);
            // Exception when claim is missing
            return string.Empty;
        }
    }
}