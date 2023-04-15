using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApi.Authorization;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;

    // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    //     throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    services.AddDbContext<ApplicationDbContext>();
    services.AddCors();
    services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// add hardcoded test user to db on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var testUser = new ApiUser
    {
        FirstName = "Test",
        LastName = "ApiUser",
        Username = "test",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("test")
    };
    context.ApiUsers.Add(testUser);
    context.SaveChanges();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");
