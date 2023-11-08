namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public static class Key
{
    public static string BrandPk(string name)
    {
        return $"BRAND#{name.ToUpper()}";
    }

    public static string BrandSk(string name)
    {
        return $"BRAND#{name.ToUpper()}";
    }

    public static string BrandContainerPk()
    {
        return "BRANDS";
    }

    public static string BrandContainerSk()
    {
        return "BRANDS";
    }

    public static string CategoryPk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public static string CategorySk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public static string DealPk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string DealSk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string DealGsi1Pk(DateTime createdAt)
    {
        return $"DEALS#{createdAt:yyyy-MM-dd}";
    }

    public static string DealGsi1Sk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string DealGsi2Pk(string brandName, DateTime createdAt)
    {
        return $"BRAND#{brandName.ToUpper()}#{createdAt:yyyy-MM-dd}";
    }

    public static string DealGsi2Sk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string DealGsi3Pk(string categoryName, DateTime createdAt)
    {
        return $"CATEGORY#{categoryName.ToUpper()}#{createdAt:yyyy-MM-dd}";
    }

    public static string DealGsi3Sk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string InteractionPk(string type, string name, string userName)
    {
        return $"{type.ToUpper()}#{name.ToLower()}#{userName.ToLower()}";
    }

    public static string InteractionSk(string type, string name, string userName)
    {
        return $"{type.ToUpper()}#{name.ToLower()}#{userName.ToLower()}";
    }

    public static string WatchPk(string name)
    {
        return $"WATCH#{name.ToLower()}";
    }

    public static string WatchSk(string userName)
    {
        return $"USER#{userName.ToLower()}";
    }

    public static string MessagePk(string userName)
    {
        return $"MESSAGES#{userName.ToLower()}";
    }

    public static string MessageSk(string messageId)
    {
        return $"MESSAGES#{messageId}";
    }

    public static string MessageGsi1Pk(string userName)
    {
        return MessagePk(userName);
    }

    public static string MessageGsi1Sk(string messageId)
    {
        return MessageSk(messageId);
    }
}
