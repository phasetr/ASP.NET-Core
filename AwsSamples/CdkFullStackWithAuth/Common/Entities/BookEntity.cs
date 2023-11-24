using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities;

[DynamoDBTable("BookCatalog")]
public class BookEntity
{
    [DynamoDBHashKey] public Guid Id { get; set; }
    [DynamoDBProperty] public string Title { get; set; } = default!;
    [DynamoDBProperty] public string? Isbn { get; set; }
    [DynamoDBProperty] public List<string>? Authors { get; set; }
    [DynamoDBIgnore] public string? CoverPage { get; set; }
}
