module Api.Anonymous

open System
open System.Security.Claims
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Shared.Errors
open Shared
open Shared.API
open Shared.JWT

type UserFromDb =
  { Id: Guid
    Name: string
    Email: string
    PwdHash: string }

let private getUserByEmail (email: string) : UserFromDb option =
  Some(
    { Id = Guid.NewGuid()
      Name = "John Doe"
      Email = email
      PwdHash = "123456" }
  )

let private userToToken (cfg: JwtConfiguration) (user: UserFromDb) : Token =
  [ Claim("id", user.Id.ToString()) ] |> List.toSeq |> createToken cfg

let private tokenToResponse (t: Token) : Response.JwtToken = { Token = t.Token }

let private login (cfg: JwtConfiguration) (req: Request.Login) =
  task {
    // let! maybeUser = req.Email |> getUserByEmail // implement such function
    return
      // maybeUser
      req.Email
      |> getUserByEmail
      |> Option.bind (fun x -> if Password.verifyPassword x.PwdHash req.Password then Some x else None)
      |> Option.map (userToToken cfg >> tokenToResponse)
      |> ServerError.ofOption (Authentication "Bad login or password")
  }

let private getAnonymousService (cfg: JwtConfiguration) = { Login = login cfg >> Async.AwaitTask }

let anonymousAPI (cfg: JwtConfiguration) =
  Remoting.createApi ()
  |> Remoting.withRouteBuilder AnonymousAPI.RouteBuilder
  |> Remoting.fromValue (getAnonymousService cfg)
  |> Remoting.withErrorHandler Remoting.errorHandler // see? we use our error handler here!
  |> Remoting.buildHttpHandler
