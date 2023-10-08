namespace BlazorJwtAuth.Common.Constants;

public static class ApiPath
{
    public const string V1Home = "api/v1/";

    public const string V1Secured = "api/v1/secured";

    public const string V1User = "api/v1/user";
    public const string V1UserAddRole = "add-role";
    public const string V1UserAddRoleFull = $"{V1User}/{V1UserAddRole}";
    public const string V1UserGetToken = "token";
    public const string V1UserGetTokenFull = $"{V1User}/{V1UserGetToken}";
    public const string V1UserLogin = "login";
    public const string V1UserLoginFull = $"{V1User}/{V1UserLogin}";
    public const string V1UserRefreshTokens = "tokens";
    public const string V1UserRefreshTokensFull = $"{V1User}/{V1UserRefreshTokens}";
    public const string V1UserRefreshToken = "refresh-token";
    public const string V1UserRefreshTokenFull = $"{V1User}/{V1UserRefreshToken}";
    public const string V1UserRegister = "register";
    public const string V1UserRegisterFull = $"{V1User}/{V1UserRegister}";
    public const string V1UserRevokeToken = "revoke-token";
    public const string V1UserRevokeTokenFull = $"{V1User}/{V1UserRefreshToken}";

    public const string V1WeatherForecastFull = "api/v1/weatherforecast";
}
