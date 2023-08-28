using BlazorJwtAuth.Client.Pages;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

public class IndexTests : TestContext
{
    [Fact]
    public void H1_StringContent()
    {
        // Arrange
        var cut = RenderComponent<Index>();

        // Act
        // Assert
        cut.Find("h1").MarkupMatches("<h1>Hello, world!</h1>");
    }
}
