using System.Security.Claims;
using Common.Entities;

namespace Service.Services.Interfaces;

public interface IClaimsService
{
    /// <summary>
    ///     入力からユーザーのクレームを作成するため、
    ///     データベースに存在しないユーザーに対してもクレームが返る。
    ///     ロールだけデータベースから取得する。
    /// </summary>
    /// <param name="applicationUser">ApplicationUserオブジェクト</param>
    /// <returns>クレームのリスト</returns>
    Task<List<Claim>> GetUserClaimsAsync(ApplicationUser applicationUser);
}
