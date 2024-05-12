module Main

open Elmish
open Elmish.React
open Feliz
open Feliz.Router

type State = { CurrentUrl: string list }

type Msg = UrlChanged of string list

let init () = { CurrentUrl = Router.currentUrl () }

let update (msg: Msg) (state: State) : State =
  match msg with
  | UrlChanged url -> { state with CurrentUrl = url }

let render (state: State) (dispatch: Msg -> unit) =
  let activePage =
    match state.CurrentUrl with
    | [] -> Html.h1 "Home"
    | [ "about" ] -> Html.h1 "About"
    | [ "contact" ] -> Html.h1 "Contact"
    | _ -> Html.h1 "Not Found"

  React.router
    [ router.onUrlChanged (UrlChanged >> dispatch)
      router.children
        [ Html.a
            [ prop.text "Home"
              prop.href (Router.format "")
              prop.style [ style.margin 5 ] ]
          Html.a
            [ prop.text "About"
              prop.href (Router.format "about")
              prop.style [ style.margin 5 ] ]
          Html.a
            [ prop.text "Contact"
              prop.href (Router.format "contact")
              prop.style [ style.margin 5 ] ]
          activePage ] ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
