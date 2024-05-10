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

let view (model: Model) dispatch =
  Html.div
    [ Html.button
        [ prop.classes [ "p-2 text-white bg-blue-500" ]
          prop.onClick (fun _ -> dispatch Toggle)
          prop.text (if model.IsOpen then "Close" else "Open") ]
      if model.IsOpen then
        Html.p [ prop.text "Hello, world!" ]
        Html.div
          [ prop.classes
              [ "fixed inset-0 transition-opacity bg-gray-800 bg-opacity-75 z-40" ]
            prop.children
              [ Html.nav
                  [ prop.classes
                      [ "fixed top-0 bottom-0 left-0 w-64 bg-white shadow-lg z-50" ]
                    prop.children
                      [ Html.ul
                          [ prop.className [ "p-4" ]
                            prop.children
                              [ Html.li [ prop.href "#"; prop.text "Home" ]
                                Html.li [ prop.href "#"; prop.text "About" ]
                                Html.li [ prop.href "#"; prop.text "Contact" ] ] ] ] ] ] ] ]

let subscribe _ = []

Program.mkProgram initialize update view
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
