module App

open Elmish
open Elmish.HMR
open Feliz
open Feliz.Router

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

let toggleButton (model: Model) dispatch =
  Html.button
    [ prop.classes [ "fixed top-6 right-6 z-10" ]
      prop.onClick (fun _ -> dispatch Toggle)
      prop.children
        [ if model.IsMenuOpen then
            Html.i [ prop.classes [ "fa-solid fa-xmark fa-2x text-white" ] ]
          else
            Html.i [ prop.classes [ "fa-solid fa-bars fa-2x" ] ] ] ]

let navMenu =
  Html.ul
    [ prop.classes
        [ "fixed top-0 right-0 z-0 w-1/4 text-white bg-blue-500 ease-linear font-bold text-xl text-center" ]
      prop.children
        [ Html.li
            [ prop.classes [ "p-3" ]
              prop.children
                [ Html.a
                    [ prop.text "Home"
                      prop.href (Router.format "")
                      prop.style [ style.margin 5 ] ] ] ]
          Html.li
            [ prop.classes [ "p-3" ]
              prop.children
                [ Html.a
                    [ prop.text "About"
                      prop.href (Router.format "about")
                      prop.style [ style.margin 5 ] ] ] ]
          Html.li
            [ prop.classes [ "p-3" ]
              prop.children
                [ Html.a
                    [ prop.text "Contact"
                      prop.href (Router.format "contact")
                      prop.style [ style.margin 5 ] ] ] ] ] ]

let header (model: Model) dispatch =
  Html.header
    [ prop.className [ "flex h-20 items-center border-y-2 p-6" ]
      prop.children
        [ Html.h1 [ prop.className [ "text-2xl font-bold" ]; prop.text "ロゴ" ]
          Html.nav
            [ prop.classes [ "right-0" ]
              prop.children
                [ toggleButton model dispatch
                  if model.IsMenuOpen then
                    navMenu ] ] ] ]

let view (model: Model) dispatch =
  let activePage =
    match model.CurrentUrl with
    | [] -> Html.h1 [ prop.classes [ "text-2xl" ]; prop.text "Home" ]
    | [ "about" ] -> Html.h1 [ prop.text "About" ]
    | [ "contact" ] -> Html.h1 [ prop.text "Contact" ]
    | _ -> Html.h1 [ prop.text "Not Found" ]

  React.router
    [ router.onUrlChanged (UrlChanged >> dispatch)
      router.children
        [ header model dispatch
          Html.main
            [ prop.className
                [ "flex min-h-screen w-6/8" ]
              prop.children [ activePage ] ] ] ]

let subscribe _ = []

Program.mkProgram initialize update view
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
