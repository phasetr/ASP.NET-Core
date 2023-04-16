using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Authentication;
using WebApi.Services;
using WebApi.Services.Authorization;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;

    /// <summary>
    ///     コンストラクター。
    /// </summary>
    /// <param name="applicationUserService"></param>
    public UsersController(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    /// <summary>
    ///     認証用のメソッド。
    ///     誰でもアクセスできる。
    /// </summary>
    /// <param name="requestModel">認証リクエスト用のモデルオブジェクト</param>
    /// <returns>トークンを含むレスポンスのJSON</returns>
    [MyAllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(Request requestModel)
    {
        var response = await _applicationUserService.AuthenticateAsync(requestModel, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    /// <summary>
    ///     リフレッシュトークンをリフレッシュする。
    /// </summary>
    /// <returns>認証レスポンス用のモデルオブジェクトのJSON</returns>
    [MyAllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = _applicationUserService.RefreshToken(refreshToken, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    /// <summary>
    ///     トークンを取り消す。
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
        // accept refresh token in request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        _applicationUserService.RevokeToken(token, IpAddress());
        return Ok(new {message = "Token revoked"});
    }

    /// <summary>
    ///     全てのユーザーを取得する.
    /// </summary>
    /// <returns>ユーザー全体を格納したJSON</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _applicationUserService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    ///     IDで指定されたユーザーを取得する.
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <returns>ユーザー情報のJSON</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var user = await _applicationUserService.GetByIdAsync(id);
        return Ok(user);
    }

    /// <summary>
    ///     データベースにアクセスしてIDで指定されたユーザーのリフレッシュトークンを取得する.
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <returns>リフレッシュトークンの値</returns>
    [HttpGet("{id}/refresh-tokens")]
    public async Task<IActionResult> GetRefreshTokens(string id)
    {
        var user = await _applicationUserService.GetByIdAsync(id);
        return Ok(user.RefreshTokens);
    }

    // helper methods

    /// <summary>
    ///     クッキーにトークンを設定する.
    ///     非同期にする必要なし.
    /// </summary>
    /// <param name="token">JWTトークン</param>
    private void SetTokenCookie(string token)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    /// <summary>
    ///     X-Forwarded-ForヘッダーからIPアドレスを取得する.
    ///     非同期にする必要なし.
    /// </summary>
    /// <returns>X-Forwarded-Forヘッダーから取得したIPアドレス</returns>
    private string IpAddress()
    {
        // get source ip address for the current request
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var address))
            return address;
        return HttpContext.Connection.RemoteIpAddress != null
            ? HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
            : "null";
    }
}
