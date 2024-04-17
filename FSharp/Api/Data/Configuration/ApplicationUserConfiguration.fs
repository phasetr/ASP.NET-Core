namespace Api.Data.Configuration

open Common.Constants
open Common.Entities
open Microsoft.AspNetCore.Identity
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Metadata.Builders

type ApplicationUserConfiguration() =
  static let hashString = "dafldjalfjsafJAF"
  static let hasher = PasswordHasher<string>()

  static let defaultUser =
    ApplicationUser(
      Id = $"{Authorization.DefaultUsername}Id",
      UserName = Authorization.DefaultUsername,
      NormalizedUserName = Authorization.DefaultUsername.ToUpper(),
      Email = Authorization.DefaultEmail,
      NormalizedEmail = Authorization.DefaultEmail.ToUpper(),
      EmailConfirmed = true,
      FirstName = "First",
      LastName = "Last",
      PhoneNumberConfirmed = true,
      PasswordHash = hasher.HashPassword(hashString, Authorization.DefaultPassword)
    )

  interface IEntityTypeConfiguration<ApplicationUser> with
    member _.Configure(builder: EntityTypeBuilder<ApplicationUser>) =
      builder.HasData(defaultUser) |> ignore
      builder.HasData(
        ApplicationUser(
          Id = "adminId",
          UserName = "admin",
          NormalizedUserName = "ADMIN",
          Email = "admin@phasetr.com",
          NormalizedEmail = "ADMIN@PHASETR.COM",
          EmailConfirmed = true,
          FirstName = "admin first",
          LastName = "admin last",
          PhoneNumberConfirmed = true,
          PasswordHash = hasher.HashPassword(hashString, "adminpass")
        )
      )
      |> ignore
