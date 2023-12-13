namespace Common;

public interface IOpenAiRequest
{
    /// <summary>
    /// OpenAIへのリクエストを作成してレスポンスをテキストで受け取る
    /// </summary>
    /// <param name="textContent"></param>
    /// <returns>OpenAI APIからの戻り値の文字列</returns>
    public Task<string> CreateChatAsync(string textContent);
}
