namespace Api

#nowarn "20"

open System
open Api.Data
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

module Program =
  [<EntryPoint>]
  let main args =
    let builder = WebApplication.CreateBuilder(args)

    let connectionString =
      builder.Configuration.GetConnectionString("DefaultConnection")
      |> function
        | null
        | "" -> raise (InvalidOperationException("Connection string 'DefaultConnection' not found."))
        | cs -> cs

    builder.Services.AddDbContext<ApplicationDbContext>(fun options -> options.UseSqlite(connectionString) |> ignore)

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
