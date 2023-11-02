using Amazon.DynamoDBv2.DataModel;

namespace Entities.DynamoDb;

/// <summary>
///     Map the Book Class to DynamoDb Table
///     To learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DeclarativeTagsList.html
/// </summary>
[DynamoDBTable("sam-appBookCatalog")]
public class Book
{
    /// <summary>
    ///     Map c# types to DynamoDb Columns
    ///     to learn more visit
    ///     https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/MidLevelAPILimitations.SupportedTypes.html
    /// </summary>
    [DynamoDBHashKey] // Partition key
    public Guid Id { get; set; }

    [DynamoDBProperty] public string Title { get; set; } = default!;

    [DynamoDBProperty] public string? Isbn { get; set; }

    [DynamoDBProperty] // String Set datatype
    public List<string>? Authors { get; set; }

    [DynamoDBIgnore] public string? CoverPage { get; set; }
}
