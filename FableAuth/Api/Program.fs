module Api.App

open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting

[<EntryPoint>]
let main _ =
  Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(fun webHostBuilder ->
      webHostBuilder
        .UseStartup(typeof<Startup.Startup>)
        .UseUrls([| "http://0.0.0.0:6500" |])
        .UseWebRoot("public")
      |> ignore)
    .Build()
    .Run()

  0
