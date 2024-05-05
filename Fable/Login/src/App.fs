[<RequireQualifiedAccess>]
module App

open Elmish

[<RequireQualifiedAccess>]
type Page =
  | Login of Login.State
  | Home of Home.State

type State = { CurrentPage: Page }

type Msg =
  | LoginMsg of Login.Msg
  | HomeMsg of Home.Msg

let init () =
  let loginState, loginCmd = Login.init ()
  { CurrentPage = Page.Login loginState }, Cmd.map LoginMsg loginCmd

let update (msg: Msg) (state: State) =
  match msg, state.CurrentPage with
  | LoginMsg loginMsg, Page.Login loginState ->
    match loginMsg with
    | Login.UserLoggedIn user ->
      let homeState, homeCmd = Home.init user

      { state with CurrentPage = Page.Home homeState }, Cmd.map HomeMsg homeCmd
    | loginMsg ->
      let loginState, loginCmd = Login.update loginMsg loginState

      { state with CurrentPage = Page.Login loginState }, Cmd.map LoginMsg loginCmd
  | HomeMsg homeMsg, _ ->
    // | HomeMsg homeMsg, Page.Home homeState ->
    match homeMsg with
    | Home.Msg.Logout -> init ()
  // | _ ->
  //   let homeState, homeCmd = Home.update homeMsg homeState
  //   { state with
  //       CurrentPage = Page.Home homeState },
  //   Cmd.map HomeMsg homeCmd
  | _, _ -> state, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  match state.CurrentPage with
  | Page.Login loginState -> Login.render loginState (LoginMsg >> dispatch)
  | Page.Home homeState -> Home.render homeState (HomeMsg >> dispatch)
