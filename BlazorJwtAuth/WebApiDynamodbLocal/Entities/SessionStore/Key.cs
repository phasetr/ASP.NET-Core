namespace WebApiDynamodbLocal.Entities.SessionStore;

public static class Key
{
    public static string SessionPk(string sessionId)
    {
        return $"{nameof(Session).ToUpper()}#{sessionId}";
    }
    
    public static string SessionSk(string sessionId)
    {
        return $"{nameof(Session).ToUpper()}#{sessionId}";
    }

    public static string SessionGsi1Pk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }
    
    public static string SessionGsi1Sk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }
}
