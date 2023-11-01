using CopilotRequestDriven.Data;
using CopilotRequestDriven.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
                      throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
 */
var secretId = Environment.GetEnvironmentVariable("WEBCLUSTER_SECRET");
Console.WriteLine($"secretId: {secretId}");
var secretArn = Environment.GetEnvironmentVariable("WEBCLUSTER_SECRET_ARN");
Console.WriteLine($"secretArn: {secretArn}");
var dbConnection = new DbConnectionStringService();
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = secretArn is null
    ? defaultConnection ?? "should not empty"
    : await dbConnection.GetConnectionString(secretArn);
Console.WriteLine($"Connection String: {connectionString}");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
