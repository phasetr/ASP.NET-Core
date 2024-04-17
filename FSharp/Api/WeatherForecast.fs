namespace Api

open System
open Api.Data.Configuration

module Test =
  let a = ApplicationUserConfiguration

type WeatherForecast =
  { Date: DateTime
    TemperatureC: int
    Summary: string }
  member this.TemperatureF = 32.0 + (float this.TemperatureC / 0.5556)
