namespace Api.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type WeatherForecast =
  { Date: DateTime
    TemperatureC: int
    Summary: string }

  member this.TemperatureF = 32.0 + (float this.TemperatureC / 0.5556)

[<ApiController>]
[<Route("api/[controller]")>]
type WeatherForecastController(logger: ILogger<WeatherForecastController>) =
  inherit ControllerBase()

  let summaries =
    [| "Freezing"
       "Bracing"
       "Chilly"
       "Cool"
       "Mild"
       "Warm"
       "Balmy"
       "Hot"
       "Sweltering"
       "Scorching" |]

  [<HttpGet>]
  member _.Get() =
    logger.LogInformation("Getting weather forecast")
    let rng = Random()

    [| for index in 0..4 ->
         { Date = DateTime.Now.AddDays(float index)
           TemperatureC = rng.Next(-20, 55)
           Summary = summaries.[rng.Next(summaries.Length)] } |]
