using System.Text.Json;
using Amazon.SecretsManager.Extensions.Caching;
using CopilotRequestDriven.Models;

namespace CopilotRequestDriven.Services;

public class DbConnectionStringService
{
    public async Task<string> GetConnectionString(string secretArn)
    {
        var cache = new SecretsManagerCache();
        string secretString;
        try
        {
            secretString = await cache.GetSecretString(secretArn);
            if (secretString == null)
            {
                Console.WriteLine("secretString is null");
                return "Cannot get a secret string";
            }
        }
        catch (Exception e)
        {
            // ジョブの場合は直接JSONの値が入るようでHTTPアクセスでエラーが出るため`secretArn`で置き換える
            Console.WriteLine(e.StackTrace);
            secretString = secretArn;
        }
        Console.WriteLine($"secretString: {secretString}");
        var secret = JsonSerializer.Deserialize<AwsAuroraSecret>(secretString);
        return
            $"User ID={secret!.username};Password={secret.password};Host={secret.host};Port={secret.port};Database={secret.dbname};";
    }
}
