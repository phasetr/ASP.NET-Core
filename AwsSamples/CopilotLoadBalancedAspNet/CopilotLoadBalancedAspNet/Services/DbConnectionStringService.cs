using System.Text.Json;
using CopilotLoadBalancedAspNet.Models;

namespace CopilotLoadBalancedAspNet.Services;

public class DbConnectionStringService
{
    /// <summary>
    ///     データベースへの接続文字列を取得するメソッド。
    ///     まず環境変数から取得できるか確認し、取得できない場合はAWS Secrets Managerから取得する。
    ///     開発環境では`appsettings.json`から取得する。
    /// </summary>
    /// <param name="aspDotNetCoreEnvironment">環境変数`ASPNETCORE_ENVIRONMENT`</param>
    /// <param name="clusterSecretEnvironmentVariable">環境変数`ASPDOTNETCLUSTER_SECRET`：AWS上でしか値を持たない</param>
    /// <param name="defaultConnection">ローカル用の接続文字列</param>
    /// <returns>データベース接続文字列</returns>
    public static string GetConnectionStringByLoadBalancer(string? aspDotNetCoreEnvironment,
        string? clusterSecretEnvironmentVariable, string? defaultConnection)
    {
        Console.WriteLine("👺 aspDotNetCoreEnvironment null check: {0}",
            aspDotNetCoreEnvironment is null ? "👺NULL" : "not null");
        Console.WriteLine("👺 aspDotNetCoreEnvironment: {0}", aspDotNetCoreEnvironment);
        Console.WriteLine("👺 clusterSecretEnvironmentVariable null check: {0}",
            clusterSecretEnvironmentVariable is null ? "👺NULL" : "not null");
        Console.WriteLine("👺 clusterSecretEnvironmentVariable: {0}", clusterSecretEnvironmentVariable);
        Console.WriteLine("👺 defaultConnection null check: {0}", defaultConnection is null ? "👺NULL" : "not null");
        Console.WriteLine("👺 defaultConnection: {0}", defaultConnection);

        if (clusterSecretEnvironmentVariable is null)
        {
            Console.WriteLine("👺`clusterSecretEnvironmentVariable` is NULL");
            return defaultConnection ?? "should not empty";
        }

        try
        {
            var secretJson = JsonSerializer.Deserialize<AwsAuroraSecret>(clusterSecretEnvironmentVariable);
            if (secretJson is null) throw new Exception("👺secretJson is null");
            return
                $"User ID={secretJson.username};Password={secretJson.password};Host={secretJson.host};Port={secretJson.port};Database={secretJson.dbname};";
        }
        catch (Exception e)
        {
            Console.WriteLine("👺 Cannot deserialize a aws secret string to json.");
            Console.WriteLine("👺 {0}", e.StackTrace);
            throw;
        }
    }
}
