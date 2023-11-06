using MediaLibrary.Api.Data;
using MediaLibrary.Api.Helper;
using MediaLibrary.Api.Services;
using MediaLibrary.Common.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<MediaLibraryDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
#if DEBUG
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
#endif
});

// CORS設定
var clientUrl = builder.Configuration["ClientUrl"];
builder.Services.AddCors(o => o.AddPolicy(clientUrl, corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddTransient<MovieService>();
builder.Services.AddTransient<PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(clientUrl);
// app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapGet("/", () => "Hello World!");
app.MapGet("/weather-forecast/list", () => new WeatherForecastModel[]
{
    new()
    {
        Date = DateTime.Now,
        TemperatureC = 25,
        Summary = "Summary"
    },
    new()
    {
        Date = new DateTime(2020, 1, 1, 0, 0, 0),
        TemperatureC = 0,
        Summary = "Summary2"
    }
});

app.Run();

// 統合テストのために追加
// ReSharper disable once ClassNeverInstantiated.Global
namespace MediaLibrary.Api
{
    public class Program
    {
    }
}
