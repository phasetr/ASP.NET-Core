module App

open System
open Elmish
open Elmish.HMR
open Fable.Core
open Feliz

type State =
  { Count: int
    Loading: bool
    Current: DateTime
    Enabled: bool }

type Msg =
  | Increment
  | Decrement
  | IncrementDelayed
  | Tick of DateTime
  | Toggle of Enabled: bool

let init () =
  { Count = 0
    Loading = false
    Current = DateTime.MinValue
    Enabled = true },
  Cmd.none

let update msg state =
  match msg with
  | Increment ->
    { state with
        Count = state.Count + 1
        Loading = false },
    Cmd.none
  | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
  | IncrementDelayed when state.Loading -> state, Cmd.none
  | IncrementDelayed ->
    let incrementDelayedCmd (dispatch: Msg -> unit) : unit =
      async {
        do! Async.Sleep 1000
        dispatch Increment
      }
      |> Async.StartImmediate

    { state with Loading = true }, Cmd.ofEffect incrementDelayedCmd
  | Tick current -> { state with Current = current }, Cmd.none
  | Toggle enabled -> { state with Enabled = enabled }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  let timestamp = state.Current.ToString("yyyy-MM-dd HH:mm:ss")
  let content = if state.Loading then Html.h2 "Loading..." else Html.h2 $"Current count: {state.Count}"

  Html.div
    [ content
      Html.button [ prop.onClick (fun _ -> dispatch Increment); prop.text "Increment" ]
      Html.button [ prop.onClick (fun _ -> dispatch Decrement); prop.text "Decrement" ]
      Html.button
        [ prop.disabled state.Loading
          prop.onClick (fun _ -> dispatch IncrementDelayed)
          prop.text "Increment Delayed" ]
      Html.div
        [ Html.div [ Html.text $"Current time: %A{timestamp}" ]
          Html.div
            [ Html.label
                [ prop.children
                    [ Html.input
                        [ prop.type' "checkbox"
                          prop.isChecked state.Enabled
                          prop.onCheckedChange (fun b -> dispatch (Toggle b)) ]
                      Html.text " enabled" ] ] ] ] ]

let timer _ =
  let start dispatch =
    let interValid = JS.setInterval (fun _ -> dispatch (Tick DateTime.Now)) 1000

    { new IDisposable with
        member _.Dispose() = JS.clearInterval interValid }

  start

let subscribe state =
  [ if state.Enabled then
      [ "timer" ], timer Tick ]

Program.mkProgram init update render
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
