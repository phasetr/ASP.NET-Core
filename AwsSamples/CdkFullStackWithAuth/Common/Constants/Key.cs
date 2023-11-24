using KsuidDotNet;

namespace Common.Constants;

public class Key
{
    /// <summary>
    ///     Ksuidを生成する。
    /// </summary>
    /// <param name="dateTime">Ksuid生成用の日時</param>
    /// <returns></returns>
    public static string GenerateKsuid(DateTime dateTime)
    {
        return Ksuid.NewKsuid(dateTime);
    }

    /// <summary>
    ///     店舗IDで生成する。
    ///     商品IDではない点に注意する。
    /// </summary>
    /// <param name="shopId">店舗ID</param>
    /// <returns></returns>
    public static string ProductPk(string shopId)
    {
        return $"PRODUCT#{shopId}";
    }

    /// <summary>
    ///     商品IDで生成する。
    /// </summary>
    /// <param name="productId">商品ID</param>
    /// <returns></returns>
    public static string ProductSk(string productId)
    {
        return $"PRODUCT#{productId}";
    }

    /// <summary>
    ///     店舗のPK。
    /// </summary>
    /// <param name="shopId">店舗ID</param>
    /// <returns></returns>
    public static string ShopPk(string shopId)
    {
        return $"SHOP#{shopId}";
    }

    /// <summary>
    ///     店舗のSK。
    /// </summary>
    /// <param name="shopId">店舗ID</param>
    /// <returns></returns>
    public static string ShopSk(string shopId)
    {
        return $"SHOP#{shopId}";
    }

    /// <summary>
    ///     ユーザーIDで生成。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    public static string UserPk(string userId)
    {
        return $"USER#{userId}";
    }

    /// <summary>
    ///     ユーザーIDで生成。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    public static string UserSk(string userId)
    {
        return $"USER#{userId}";
    }
}
