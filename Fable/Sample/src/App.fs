module App

open Elmish
open Elmish.HMR
open Feliz

type Model = { IsOpen: bool }
type Msg = Toggle

let initialize () = { IsOpen = false }, Cmd.none

let update msg model =
  match msg with
  | Toggle -> { model with IsOpen = not model.IsOpen }, Cmd.none

let toggleButton (model: Model) dispatch =
  Html.button
    [ prop.classes [ "fixed top-6 right-6 z-10" ]
      prop.onClick (fun _ -> dispatch Toggle)
      prop.children
        [ if model.IsOpen then
            Html.i [ prop.classes [ "fa-solid fa-xmark fa-2x text-white" ] ]
          else
            Html.i [ prop.classes [ "fa-solid fa-bars fa-2x" ] ] ] ]

let navMenu =
  Html.ul
    [ prop.classes
        [ "fixed top-0 left-0 z-0 w-full text-white bg-blue-500 ease-linear font-bold text-xl text-center" ]
      prop.children
        [ Html.li
            [ prop.classes [ "p-3" ]
              prop.children [ Html.a [ prop.href "#"; prop.text "Home" ] ] ]
          Html.li
            [ prop.classes [ "p-3" ]
              prop.children [ Html.a [ prop.href "#"; prop.text "About" ] ] ]
          Html.li
            [ prop.classes [ "p-3" ]
              prop.children [ Html.a [ prop.href "#"; prop.text "Contact" ] ] ] ] ]

let view (model: Model) dispatch =
  Html.header
    [ prop.className [ "flex h-20 items-center border-y-2 p-6" ]
      prop.children
        [ Html.h1 [ prop.className [ "text-2xl font-bold" ]; prop.text "ロゴ" ]
          Html.nav
            [ prop.children
                [ toggleButton model dispatch
                  if model.IsOpen then
                    navMenu ] ] ] ]

let subscribe _ = []

Program.mkProgram initialize update view
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
