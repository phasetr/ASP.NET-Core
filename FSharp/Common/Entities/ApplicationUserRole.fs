namespace Common.Entities

open Microsoft.AspNetCore.Identity

type ApplicationUserRole() =
  inherit IdentityUserRole<string>()
