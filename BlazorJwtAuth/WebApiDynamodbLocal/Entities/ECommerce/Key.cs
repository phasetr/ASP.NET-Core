namespace WebApiDynamodbLocal.Entities.ECommerce;

public static class Key
{
    public static string CustomerPk(string userName)
    {
        return $"CUSTOMER#{userName}";
    }

    public static string CustomerSk(string userName)
    {
        return $"CUSTOMER#{userName}";
    }

    public static string CustomerEmailPk(string email)
    {
        return $"CUSTOMEREMAIL#{email}";
    }

    public static string CustomerEmailSk(string email)
    {
        return $"CUSTOMEREMAIL#{email}";
    }

    public static string OrderPk(string userName)
    {
        return $"CUSTOMER#{userName}";
    }

    public static string OrderSk(string orderId)
    {
        return $"#ORDER#{orderId}";
    }

    public static string OrderGsi1Pk(string orderId)
    {
        return $"ORDER#{orderId}";
    }

    public static string OrderGsi1Sk(string orderId)
    {
        return $"ORDER#{orderId}";
    }

    public static string OrderItemPk(string orderId, string itemId)
    {
        return $"ORDER#{orderId}#ITEM#{itemId}";
    }

    public static string OrderItemSk(string orderId, string itemId)
    {
        return $"ORDER#{orderId}#ITEM#{itemId}";
    }

    public static string OrderItemGsi1Pk(string orderId)
    {
        return $"ORDER#{orderId}";
    }

    public static string OrderItemGsi1Sk(string itemId)
    {
        return $"ITEM#{itemId}";
    }
}
