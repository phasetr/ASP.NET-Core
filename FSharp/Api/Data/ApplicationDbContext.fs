namespace Api.Data

open Api.Data.Configuration
open Common.Entities
open Microsoft.AspNetCore.Identity.EntityFrameworkCore
open Microsoft.EntityFrameworkCore

type ApplicationDbContext(options: DbContextOptions<ApplicationDbContext>) =
  inherit IdentityDbContext<ApplicationUser>(options)
  member this.ApplicationRoles = base.Set<ApplicationRole>()
  member this.ApplicationUsers = base.Set<ApplicationUser>()
  member this.ApplicationUserRoles = base.Set<ApplicationUserRole>()

  override this.OnModelCreating(builder: ModelBuilder) =
    base.OnModelCreating(builder)

    builder
      .ApplyConfiguration(ApplicationUserConfiguration())
      .ApplyConfiguration(ApplicationRoleConfiguration())
      .ApplyConfiguration(ApplicationUserRoleConfiguration())
    |> ignore
