module Api.Secured

open System
open Api.Anonymous
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe.Core
open Microsoft.AspNetCore.Http
open Shared.Errors
open Shared.API

let private getUserById (i: Guid) : UserFromDb option =
  Some(
    { Id = i
      Name = "John Doe"
      Email = "user@phasetr.com"
      PwdHash = "123456" }
  )

let private userToResponse (user: UserFromDb) : Response.UserInfo = { Name = user.Name; Email = user.Email }

let private getUserInfo userId () =
  task {
    // let! maybeUser = userId |> getUserById
    return
      // maybeUser
      userId
      |> getUserById
      |> Option.map userToResponse
      |> ServerError.ofOption (Authentication "User account not found")
  }

let private getSecuredService (ctx: HttpContext) =
  let userId = ctx.User.Claims |> Seq.find (fun x -> x.Type = "id") |> (fun x -> Guid x.Value)
  { GetUserInfo = getUserInfo userId >> Async.AwaitTask }

let securedAPI: HttpFunc -> HttpContext -> HttpFuncResult =
  Remoting.createApi ()
  |> Remoting.withRouteBuilder SecuredAPI.RouteBuilder
  |> Remoting.fromContext getSecuredService // <-- we need context here
  |> Remoting.withErrorHandler Remoting.errorHandler // see? we use our error handler here!
  |> Remoting.buildHttpHandler
