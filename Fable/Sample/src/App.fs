module App

open Elmish
open Elmish.HMR
open Feliz
open Feliz.Router
open Sample.Global.Header
open Sample.Global.LeftMenu
open Sample.Global.Footer
open Sample.Home.Main

type Model =
  { IsMenuOpen: bool
    CurrentUrl: string list }

type Msg =
  | Toggle
  | UrlChanged of string list

let initialize () =
  { IsMenuOpen = false
    CurrentUrl = Router.currentUrl () },
  Cmd.none

let update msg model =
  match msg with
  | Toggle ->
    { model with
        IsMenuOpen = not model.IsMenuOpen },
    Cmd.none
  | UrlChanged url -> { model with CurrentUrl = url }, Cmd.none

let view (model: Model) dispatch =
  let activePage =
    match model.CurrentUrl with
    | [] -> main
    | [ "about" ] ->
      Html.h1 [ prop.className "text-3xl font-bold mb-4"; prop.text "About" ]
    | [ "contact" ] ->
      Html.h1 [ prop.className "text-3xl font-bold mb-4"; prop.text "Contact" ]
    | _ ->
      Html.h1
        [ prop.className "text-3xl font-bold mb-4"; prop.text "Not Found" ]

  React.router
    [ router.onUrlChanged (UrlChanged >> dispatch)
      router.children
        [ Html.main
            [ prop.className [ "flex min-h-screen w-6/8" ]
              prop.children
                [ Html.div
                    [ prop.className "flex min-h-screen"
                      prop.children
                        [ leftMenu
                          Html.div
                            [ prop.className "flex-1 overflow-y-auto"
                              prop.children [ header; activePage; footer ] ] ] ] ] ] ] ]

let subscribe _ = []

Program.mkProgram initialize update view
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
