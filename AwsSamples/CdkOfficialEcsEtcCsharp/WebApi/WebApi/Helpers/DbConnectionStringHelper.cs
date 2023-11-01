using System.Text.Json;
using WebApi.Models;

namespace WebApi.Helpers;

public static class DbConnectionStringHelper
{
    /// <summary>
    ///     データベース文字列を取得する。
    /// </summary>
    /// <param name="clusterSecretEnvironmentVariable">環境変数から取得した文字列</param>
    /// <returns>データベース接続文字列</returns>
    public static string GetConnectionString(string? clusterSecretEnvironmentVariable)
    {
        if (clusterSecretEnvironmentVariable is null)
        {
            Console.WriteLine("👺 The argument is null.");
            return string.Empty;
        }

        var secretJson = JsonSerializer.Deserialize<AwsAuroraSecret>(clusterSecretEnvironmentVariable);
        if (secretJson is null)
        {
            Console.WriteLine("👺 Cannot deserialize a aws secret string to json.");
            return string.Empty;
        }

        return
            $"User ID={secretJson.username};Password={secretJson.password};Host={secretJson.host};Port={secretJson.port};Database={secretJson.dbname};";
    }
}
