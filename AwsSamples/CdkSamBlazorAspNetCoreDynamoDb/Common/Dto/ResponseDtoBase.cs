namespace Common.Dto;

public class ResponseDtoBase
{
    /// <summary>
    ///     主にエラー時のメッセージを格納する。
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///     リクエストの基本的な内容の成否を判定する。
    /// </summary>
    public bool IsSucceed { get; set; }
}
