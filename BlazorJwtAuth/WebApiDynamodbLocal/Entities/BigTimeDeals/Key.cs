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
}
