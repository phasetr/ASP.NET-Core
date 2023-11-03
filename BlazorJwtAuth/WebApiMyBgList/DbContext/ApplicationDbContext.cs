using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApiUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<BoardGame> BoardGames => Set<BoardGame>();
    public DbSet<Domain> Domains => Set<Domain>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<BoardGamesDomains> BoardGamesDomains => Set<BoardGamesDomains>();
    public DbSet<BoardGamesMechanics> BoardGamesMechanics => Set<BoardGamesMechanics>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 複合主キーを設定
        modelBuilder.Entity<BoardGamesDomains>()
            .HasKey(i => new {i.BoardGameId, i.DomainId});

        modelBuilder.Entity<BoardGamesDomains>()
            .HasOne(x => x.BoardGame)
            .WithMany(y => y.BoardGamesDomains)
            .HasForeignKey(f => f.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGamesDomains>()
            .HasOne(o => o.Domain)
            .WithMany(m => m.BoardGamesDomains)
            .HasForeignKey(f => f.DomainId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // 複合主キーを設定
        modelBuilder.Entity<BoardGamesMechanics>()
            .HasKey(i => new {i.BoardGameId, i.MechanicId});

        modelBuilder.Entity<BoardGamesMechanics>()
            .HasOne(x => x.BoardGame)
            .WithMany(y => y.BoardGamesMechanics)
            .HasForeignKey(f => f.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BoardGamesMechanics>()
            .HasOne(o => o.Mechanic)
            .WithMany(m => m.BoardGamesMechanics)
            .HasForeignKey(f => f.MechanicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Moderator, Administrator ロールを作成
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "Moderator",
                NormalizedName = "MODERATOR"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );
        // テストユーザーを作成
        var hasher = new PasswordHasher<string>();
        const string hashString = "hashString";
        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "1",
                UserName = "TestModerator",
                NormalizedUserName = "TESTMODERATOR",
                Email = "TestModerator@phasetr.com",
                PasswordHash = hasher.HashPassword(hashString, "TestModerator")
            });
        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "2",
                UserName = "TestAdministrator",
                NormalizedUserName = "TESTADMINISTRATOR",
                Email = "TestAdministrator@phasetr.com",
                PasswordHash = hasher.HashPassword(hashString, "TestAdministrator")
            });
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new List<IdentityUserRole<string>>
        {
            new() {UserId = "1", RoleId = "1"},
            new() {UserId = "2", RoleId = "1"},
            new() {UserId = "2", RoleId = "2"}
        });
    }
}
