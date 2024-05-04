module Fable.Login

open Feliz
open Feliz.UseDeferred
open Shared.API
open Shared.Errors

let displayStronglyTypedError =
  function
  // choose how to display errors based on your needs
  | ServerError.Exception x -> Html.text x
  | ServerError.Authentication x -> Html.div x

[<ReactComponent>]
let LoginView () =
  let userLogin: Request.Login = { Email = ""; Password = "" }
  let loginForm, setLoginForm = React.useState userLogin
  let loginReq, setLoginReq = React.useState Deferred.HasNotStartedYet
  let login = React.useDeferredCallback ((fun _ -> Server.anonymousAPI.Login loginForm), setLoginReq)

  let result =
    match loginReq with
    | Deferred.HasNotStartedYet
    | Deferred.InProgress -> Html.none
    | Deferred.Failed ex -> ex |> Server.exnToError |> displayStronglyTypedError
    | Deferred.Resolved resp ->
      Browser.WebStorage.localStorage.setItem ("token", resp.Token) // store for later usage
      Html.text "YOU ARE IN!"

  Html.div [ Html.input [ prop.type'.text
                          prop.onTextChange (fun x -> { loginForm with Email = x } |> setLoginForm) ]
             Html.input [ prop.type'.password
                          prop.onTextChange (fun x -> { loginForm with Password = x } |> setLoginForm) ]
             Html.button [ prop.text "LOGIN"
                           if Deferred.inProgress loginReq then prop.disabled true
                           prop.onClick login ] ]
