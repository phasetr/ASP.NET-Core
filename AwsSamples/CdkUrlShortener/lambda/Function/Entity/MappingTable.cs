using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;

namespace Function.Entity;

[DynamoDBTable("mapping-table")]
public class MappingTable
{
    [JsonPropertyName("id")]
    [DynamoDBHashKey]
    public string Id { get; set; } = default!;

    [JsonPropertyName("targetUrl")]
    [DynamoDBRangeKey]
    public string TargetUrl { get; set; } = default!;
}
