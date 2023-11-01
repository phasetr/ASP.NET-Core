namespace CopilotRequestDriven.Models;

public class AwsAuroraSecret
{
    public string password { get; set; } = string.Empty;
    public string dbname { get; set; } = string.Empty;
    public int port { get; set; }
    public string host { get; set; } = string.Empty;
    public string username { get; set; } = string.Empty;
}
