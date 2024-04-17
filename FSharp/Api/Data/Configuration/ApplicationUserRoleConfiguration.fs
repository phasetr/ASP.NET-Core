namespace Api.Data.Configuration

open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Metadata.Builders
open Common.Entities

type ApplicationUserRoleConfiguration() =
  interface IEntityTypeConfiguration<ApplicationUserRole> with
    member _.Configure(builder: EntityTypeBuilder<ApplicationUserRole>) =
      builder.HasData(
        ApplicationUserRole(UserId = "userId", RoleId = "UserId"),
        ApplicationUserRole(UserId = "adminId", RoleId = "AdministratorId")
      )
      |> ignore
