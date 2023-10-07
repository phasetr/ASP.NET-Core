using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenService
{
    /// <summary>
    ///     ローカルストレージからトークンを取得する。
    /// </summary>
    /// <returns></returns>
    Task<TokenDto> GetTokenAsync();

    /// <summary>
    ///     ローカルストレージのトークンを削除する。
    /// </summary>
    /// <returns></returns>
    Task RemoveTokenAsync();

    /// <summary>
    ///     ローカルストレージにトークンを設定する。
    /// </summary>
    /// <param name="tokenDto"></param>
    /// <returns></returns>
    Task SetTokenAsync(TokenDto tokenDto);
}
