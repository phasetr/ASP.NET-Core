namespace Common.Dto;

public class ResponseBaseDto
{
    /// <summary>
    ///     画面表示用のメッセージ。
    ///     エラーの場合は`Errors`に格納する。
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    ///     成功・失敗を格納する。
    ///     システム内部でも実行結果判定に応用する。
    /// </summary>
    public bool Succeeded { get; set; } = true;

    /// <summary>
    ///     画面でのエラー表示以外に情報伝播にも応用する。
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = default!;
}
