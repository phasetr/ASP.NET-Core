using System.Text.Json.Serialization;

namespace WebApi.Models;

/// <summary>
///     AWS AuroraのSecret
/// </summary>
public class AwsAuroraSecret
{
    [JsonPropertyName("password")] public string Password { get; set; } = string.Empty;

    [JsonPropertyName("dbname")] public string Dbname { get; set; } = string.Empty;

    [JsonPropertyName("port")] public int Port { get; set; }

    [JsonPropertyName("host")] public string Host { get; set; } = string.Empty;

    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
}
