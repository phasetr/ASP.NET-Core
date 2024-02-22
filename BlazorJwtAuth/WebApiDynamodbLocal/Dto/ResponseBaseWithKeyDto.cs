namespace WebApiDynamodbLocal.Dto;

public class ResponseBaseWithKeyDto : Common.Dto.ResponseBaseDto
{
    /// <summary>
    ///     特にDynamoDBのときPostで生成されたデータのキーを返したい場合に使用する。
    /// </summary>
    public string? Key { get; set; } = default!;
}
