namespace Common.Entities

open Microsoft.AspNetCore.Identity

type ApplicationUser() =
    inherit IdentityUser()
    member val FirstName: string = "" with get, set
    member val LastName: string = "" with get, set
