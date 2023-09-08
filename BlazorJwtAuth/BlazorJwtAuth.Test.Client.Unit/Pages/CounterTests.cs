using BlazorJwtAuth.Client.Pages;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

/// <summary>
///     These tests are written entirely in C#.
///     Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class CounterTests : TestContext
{
    [Fact]
    public void CounterStartsAtZero()
    {
        var cut = RenderComponent<Counter>();
        cut.Find("p").MarkupMatches("<p role=\"status\">Current count: 0</p>");
    }

    [Fact]
    public void ClickingButtonIncrementsCounter()
    {
        var cut = RenderComponent<Counter>();
        cut.Find("button").Click();
        cut.Find("p").MarkupMatches("<p role=\"status\">Current count: 1</p>");
    }
}
