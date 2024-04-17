namespace Api

#nowarn "20"

open System
open Common.Entities
open DataContext.Data
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Identity
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

module Program =
  let getConfiguration (builder: WebApplicationBuilder) item =
    match builder.Configuration[item] with
    | null
    | "" -> raise (InvalidOperationException($"{item} not found."))
    | cs -> cs

  [<EntryPoint>]
  let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services.AddControllers()
    builder.Services.AddSwaggerGen()

    // Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
    // package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
    // builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

    // Establish cookie authentication
    builder.Services
      .AddAuthentication(IdentityConstants.ApplicationScheme)
      .AddIdentityCookies()

    // Configure authorization
    builder.Services.AddAuthorizationBuilder()

    let connectionString =
      builder.Configuration.GetConnectionString("DefaultConnection")
      |> function
        | null
        | "" -> raise (InvalidOperationException("Connection string 'DefaultConnection' not found."))
        | cs -> cs

    builder.Services.AddDbContext<ApplicationDbContext>(fun options -> options.UseSqlite(connectionString) |> ignore)
    // Add identity and opt-in to endpoints
    builder.Services
      .AddIdentityCore<ApplicationUser>()
      .AddRoles<ApplicationRole>()
      .AddRoleManager<RoleManager<ApplicationRole>>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddApiEndpoints()

    // Add a CORS policy for the client
    let frontendUrl = getConfiguration builder "FrontendUrl"
    let backendUrl = getConfiguration builder "BackendUrl"

    builder.Services.AddCors(fun options ->
      options.AddPolicy(
        "wasm",
        Action<CorsPolicyBuilder>(fun builder ->
          builder
            .WithOrigins(frontendUrl, backendUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
          |> ignore)
      ))

    // Add services to the container
    builder.Services.AddEndpointsApiExplorer()

    let app = builder.Build()

    if builder.Environment.IsDevelopment() then
      app.UseSwagger()
      app.UseSwaggerUI()
    else
      app.UseExceptionHandler("/Error")
      app.UseHsts()

    app.UseHttpsRedirection()
    app.UseStaticFiles()
    app.UseRouting()

    // Create routes for the identity endpoints
    // app.MapIdentityApi<ApplicationUser>()

    // Activate the CORS policy
    app.UseCors("wasm")

    app.UseAuthentication()
    app.UseAuthorization()
    app.MapControllers()
    app.Run()

    0
