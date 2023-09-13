namespace BlazorJwtAuth.Common.Dto;

public class ResponseBaseDto
{
    /// <summary>
    ///     本番利用では不要か？
    /// </summary>
    public string Detail { get; set; } = default!;

    /// <summary>
    ///     画面表示用のメッセージ。
    ///     エラーの場合はエラーメッセージ(`ex.Message`)を格納する。
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    ///     HTTPステータスコード。
    ///     本番利用では不要か？
    /// </summary>
    public string Status { get; set; } = default!;

    /// <summary>
    ///     成功・失敗を格納する。
    ///     システム内部でも実行結果判定に応用する。
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    ///     画面でのエラー表示以外に情報伝播にも応用する。
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = default!;
}
