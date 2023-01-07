using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MvcWithApi.Data;
using MvcWithApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDbContext") ??
                      throw new InvalidOperationException("Connection string 'MyDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// API利用：`Swashbuckle.AspNetCore`が必要
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://phasetr.com/archive"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://phasetr.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://phasetr.com/archive")
        }
    });
});

var app = builder.Build();

// API利用：開発時は`Swagger`を立ち上げる
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// API利用
app.MapControllers();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();