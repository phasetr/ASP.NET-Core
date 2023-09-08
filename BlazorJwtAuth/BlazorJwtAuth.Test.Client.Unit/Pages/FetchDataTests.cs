using System;
using System.Collections.Generic;
using BlazorJwtAuth.Client.Pages;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

public class FetchDataTests : TestContext
{
    [Fact]
    public void Sample_Test()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("/sample-data/weather.json").RespondJson(new List<WeatherForecastDto>
        {
            new()
            {
                Date = new DateTime(2023, 1, 1),
                Summary = "Summary1",
                TemperatureC = 1
            },
            new()
            {
                Date = new DateTime(2023, 1, 2),
                Summary = "Summary2",
                TemperatureC = 2
            }
        });

        var cut = RenderComponent<FetchData>();
        cut.WaitForElements("tbody").MarkupMatches(
            """
            <tbody>
            <tr>
                <td>2023/01/01</td><td>1</td><td>33</td><td>Summary1</td>
            </tr>
            <tr>
                <td>2023/01/02</td><td>2</td><td>35</td><td>Summary2</td>
            </tr>
            </tbody>
            """);
    }
}
