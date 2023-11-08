namespace Common.Dto;

public class ResponseBaseWithKeyDto : ResponseBaseDto
{
    /// <summary>
    ///     特にDynamoDBのときPostで生成されたデータのキーを返したい場合に使用する。
    /// </summary>
    public string? Key { get; set; } = default!;
}
