using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Data;
using WebApi.Errors;
using WebApi.Models;
using WebApi.Models.Authentication;
using WebApi.Services.Authorization;

namespace WebApi.Services;

public interface IApplicationUserService
{
    /// <summary>
    ///     ユーザーを認証してトークンをデータベースに格納する。
    /// </summary>
    /// <param name="requestModel">リクエストで渡されたユーザー情報を格納するオブジェクト</param>
    /// <param name="ipAddress">トークンに記録するIPアドレス</param>
    /// <returns>トークンを含むJSONオブジェクト</returns>
    Task<Response> AuthenticateAsync(Request requestModel, string ipAddress);

    /// <summary>
    ///     トークンをリフレッシュしてデータベースに格納する。
    /// </summary>
    /// <param name="token">JWTトークン></param>
    /// <param name="ipAddress">新たなトークンに格納するIPアドレス</param>
    /// <returns></returns>
    Response RefreshToken(string token, string ipAddress);

    /// <summary>
    ///     リフレッシュトークンも含めてトークンを取り消す。
    /// </summary>
    /// <param name="token">JWTトークン</param>
    /// <param name="ipAddress">アクセスされたIPアドレス</param>
    void RevokeToken(string token, string ipAddress);

    /// <summary>
    ///     データベースから登録されている全ユーザーを取得する.
    /// </summary>
    /// <returns>全ユーザーのオブジェクトのリスト</returns>
    Task<IEnumerable<ApplicationUser>> GetAllAsync();

    /// <summary>
    ///     データベースから指定されたIDのユーザーを取得する.
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <returns>特定ユーザーのオブジェクト</returns>
    Task<ApplicationUser> GetByIdAsync(string id);
}

public class ApplicationUserService : IApplicationUserService
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _userManager = userManager;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }

    // インターフェイスのコメント参照
    public async Task<Response> AuthenticateAsync(Request requestModel, string ipAddress)
    {
        var user = await _context.ApplicationUsers.SingleOrDefaultAsync(x => x.UserName == requestModel.UserName);
        // validate
        if (user is null) throw new JwtAuthenticationException("UserName or password is incorrect");
        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var result = await passwordValidator.ValidateAsync(_userManager, user, requestModel.Password);
        if (!result.Succeeded) throw new JwtAuthenticationException("UserName or password is incorrect");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        user.RefreshTokens.Add(refreshToken);

        // remove old refresh tokens from applicationUser
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        await _context.SaveChangesAsync();

        return new Response(user, jwtToken, refreshToken.Token);
    }

    // インターフェイスのコメント参照
    public Response RefreshToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            RevokeDescendantRefreshTokens(refreshToken, user, ipAddress,
                $"Attempted reuse of revoked ancestor token: {token}");
            _context.Update(user);
            _context.SaveChanges();
        }

        if (!refreshToken.IsActive)
            throw new JwtAuthenticationException("Invalid token");

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from applicationUser
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();

        // generate new jwt
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new Response(user, jwtToken, newRefreshToken.Token);
    }

    // インターフェイスのコメント参照
    public void RevokeToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new JwtAuthenticationException("Invalid token");

        // revoke token and save
        RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
        _context.Update(user);
        _context.SaveChanges();
    }

    // インターフェイスのコメント参照
    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await _context.ApplicationUsers.ToListAsync();
    }

    // インターフェイスのコメント参照
    public async Task<ApplicationUser> GetByIdAsync(string id)
    {
        var user = await _context.ApplicationUsers.FindAsync(id);
        if (user == null) throw new KeyNotFoundException($"{nameof(ApplicationUser)} not found");
        return user;
    }

    // helper methods

    /// <summary>
    ///     データベースにアクセスしてトークンを持つユーザーを取得する.
    /// </summary>
    /// <param name="token">リフレッシュトークン</param>
    /// <returns>該当ユーザー</returns>
    /// <exception cref="JwtAuthenticationException"></exception>
    private ApplicationUser GetUserByRefreshToken(string token)
    {
        var user = _context.ApplicationUsers
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user == null) throw new JwtAuthenticationException("Invalid token");
        return user;
    }

    /// <summary>
    ///     与えられたリフレッシュトークンを無効にして新たなトークンを返す。
    /// </summary>
    /// <param name="refreshToken">リフレッシュトークン</param>
    /// <param name="ipAddress">トークンに記録するIPアドレス</param>
    /// <returns>リフレッシュトークン</returns>
    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    /// <summary>
    ///     アプリ設定のTTLに基づいて無効なトークンを削除する。
    /// </summary>
    /// <param name="applicationUser">トークンを削除したいユーザーのオブジェクト</param>
    private void RemoveOldRefreshTokens(ApplicationUser applicationUser)
    {
        // remove old inactive refresh tokens from applicationUser based on TTL in app settings
        applicationUser.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_appSettings.RefreshTokenTtl) <= DateTime.UtcNow);
    }

    /// <summary>
    ///     再帰的に子トークンを無効にする。
    /// </summary>
    /// <param name="refreshToken">リフレッシュトークン</param>
    /// <param name="applicationUser">取り消し対象のトークンを持つユーザー</param>
    /// <param name="ipAddress">トークンに記録するIPアドレス</param>
    /// <param name="reason">トークンの取り消し理由</param>
    private static void RevokeDescendantRefreshTokens(RefreshToken refreshToken, ApplicationUser applicationUser,
        string ipAddress,
        string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        var childToken = applicationUser.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken is {IsActive: true})
            RevokeRefreshToken(childToken, ipAddress, reason);
        else
            RevokeDescendantRefreshTokens(childToken, applicationUser, ipAddress, reason);
    }

    /// <summary>
    ///     入力のトークンが無効になるように書き換える。
    /// </summary>
    /// <param name="token">書き換えられるトークン</param>
    /// <param name="ipAddress">トークンに記録されるIPアドレス</param>
    /// <param name="reason">無効化される理由</param>
    /// <param name="replacedByToken">ReplacedByTokenに格納する値</param>
    private static void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null,
        string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
}
