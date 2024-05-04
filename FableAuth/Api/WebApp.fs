module Api.WebApp

open Giraffe
open Microsoft.AspNetCore.Authentication.JwtBearer
open Shared.JWT
open Api.Anonymous
open Api.Secured

let private mustBeLoggedIn: HttpHandler =
  requiresAuthentication (RequestErrors.UNAUTHORIZED JwtBearerDefaults.AuthenticationScheme "" "User not logged in")

let webApp (cfg: JwtConfiguration) : HttpHandler =
  choose [ anonymousAPI cfg
           mustBeLoggedIn >=> choose [ securedAPI ]
           htmlFile "public/index.html" ]
