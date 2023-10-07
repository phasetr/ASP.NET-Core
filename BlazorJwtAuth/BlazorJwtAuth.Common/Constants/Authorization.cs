namespace BlazorJwtAuth.Common.Constants;

public class Authorization
{
    public enum Roles
    {
        Administrator,
        Moderator,
        User
    }

    public const string DefaultUsername = "user";
    public const string DefaultEmail = "user@secureapi.com";
    public const string DefaultPassword = "Pa$$w0rd.";
    public const Roles DefaultRole = Roles.User;

    /// <summary>
    ///     JWTのアクセストークン名。
    ///     アクセストークンはCookieに保存される。
    ///     時間設定は`WebApi`の`Program.cs`で直接設定している。
    /// </summary>
    public const string JwtAccessTokenName = "accessToken";
}
