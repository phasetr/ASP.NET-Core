using BlazorJwtAuth.Common.DataContext.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorJwtAuth.Test.Integration;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(new SqliteConnection("DataSource=:memory:"));
            });

            var sp = services.BuildServiceProvider();
            // Scopeを作ってDbContextが使いまわされないようにする
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // DB作り直し
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SaveChanges();
        });
        builder.UseEnvironment("Development");
    }
}
