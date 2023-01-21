namespace SportsStore.Models;

public static class Constants
{
    public const int DefaultPageSize = 2;

    /// <summary>
    ///     本来は`Products`のプロパティでデータベースに入れる対象。
    ///     ここでは元の`SportsStore`のコードを壊さないようにするため一時的な定数として取り込んでいる。
    /// </summary>
    public static readonly List<string> TemporalCategories = new() {"Chess", "Soccer", "Watersports"};
}