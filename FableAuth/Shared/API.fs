module Shared.API

[<RequireQualifiedAccess>]
module Request =
  type Login = { Email: string; Password: string }

[<RequireQualifiedAccess>]
module Response =
  type JwtToken = { Token: string }

  type UserInfo = { Name: string; Email: string }

type AnonymousAPI =
  { Login: Request.Login -> Async<Response.JwtToken> } // note no Result here!
  static member RouteBuilder _ m = $"/api/anonymous/%s{m}"

type SecuredAPI =
  { GetUserInfo: unit -> Async<Response.UserInfo> } // note no Result here!
  static member RouteBuilder _ m = $"/api/secured/%s{m}"
