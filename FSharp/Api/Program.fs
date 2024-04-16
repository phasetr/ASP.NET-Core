namespace Api

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
  [<EntryPoint>]
  let main args =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddControllers()
    builder.Services.AddSwaggerGen()

    let app = builder.Build()

    if builder.Environment.IsDevelopment() then
      app.UseSwagger()
      app.UseSwaggerUI()
    else
      app.UseExceptionHandler("/Error")
      app.UseHsts()

    app.UseHttpsRedirection()
    app.UseAuthorization()
    app.MapControllers()
    app.Run()

    0
