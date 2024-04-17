namespace Api.Data.Configuration

open Common.Constants
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Metadata.Builders
open Common.Entities

type ApplicationRoleConfiguration() =
  interface IEntityTypeConfiguration<ApplicationRole> with
    member _.Configure(builder: EntityTypeBuilder<ApplicationRole>) =
      builder.HasData(
        [| ApplicationRole(
             Id = $"{Authorization.Roles.Administrator.ToString()}Id",
             Name = Authorization.Roles.Administrator.ToString(),
             NormalizedName = Authorization.Roles.Administrator.ToString().ToUpper()
           ),
           ApplicationRole(
             Id = $"{Authorization.Roles.Moderator.ToString()}Id",
             Name = Authorization.Roles.Moderator.ToString(),
             NormalizedName = Authorization.Roles.Moderator.ToString().ToUpper()
           ),
           ApplicationRole(
             Id = $"{Authorization.Roles.User.ToString()}Id",
             Name = Authorization.Roles.User.ToString(),
             NormalizedName = Authorization.Roles.User.ToString().ToUpper()
           ) |]
      ) |> ignore
