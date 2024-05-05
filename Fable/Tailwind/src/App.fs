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
  | DecrementDelayed
  | Tick of DateTime
  | Toggle of Enabled: bool

let init () =
  { Count = 0
    Loading = false
    Current = DateTime.MinValue
    Enabled = true },
  Cmd.none

module Cmd =
  let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
    let delayedCmd (dispatch: 'msg -> unit) : unit =
      async {
        let! msg = operation
        dispatch msg
      }
      |> Async.StartImmediate

    Cmd.ofEffect delayedCmd

let delayedMsg (delay: int) (msg: Msg) : Cmd<Msg> =
  async {
    do! Async.Sleep delay
    return msg
  }
  |> Cmd.fromAsync

let delayedIncrement =
  async {
    do! Async.Sleep 1000
    return Increment
  }

let update msg state =
  match msg with
  | Increment ->
    { state with
        Count = state.Count + 1
        Loading = false },
    Cmd.none
  | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
  | IncrementDelayed when state.Loading -> state, Cmd.none
  | IncrementDelayed -> { state with Loading = true }, Cmd.fromAsync delayedIncrement
  | DecrementDelayed -> state, delayedMsg 1000 Decrement
  | Tick current -> { state with Current = current }, Cmd.none
  | Toggle enabled -> { state with Enabled = enabled }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  let timestamp = state.Current.ToString("yyyy-MM-dd HH:mm:ss")

  let content =
    if state.Loading then Html.h2 "Loading..." else Html.h2 $"Current count: {state.Count}"

  Html.div [ content
             Html.h1 [ prop.classes [ "text-3xl font-bold underline" ]
                       prop.text "Hello world!" ]
             Html.button [ prop.classes [ "bg-red-600 hover:bg-red-500 text-white rounded px-4 py-2" ]
                           prop.onClick (fun _ -> dispatch Increment)
                           prop.text "Increment" ]
             Html.button [ prop.classes [ "bg-green-600 hover:bg-green-500 text-white rounded px-4 py-2" ]
                           prop.onClick (fun _ -> dispatch Decrement)
                           prop.text "Decrement" ]
             Html.button [ prop.classes [ "bg-blue-600 hover:bg-blue-500 text-white rounded px-4 py-2" ]
                           prop.disabled state.Loading
                           prop.onClick (fun _ -> dispatch IncrementDelayed)
                           prop.text "Increment Delayed" ]
             Html.div [ Html.div [ Html.text $"Current time: %A{timestamp}" ]
                        Html.div [ Html.label [ prop.children [ Html.input [ prop.type' "checkbox"
                                                                             prop.isChecked
                                                                               state.Enabled
                                                                             prop.onCheckedChange
                                                                               (fun b ->
                                                                                 dispatch (Toggle b)) ]
                                                                Html.text " enabled" ] ] ] ] ]

let timer _ =
  let start dispatch =
    let interValid = JS.setInterval (fun _ -> dispatch (Tick DateTime.Now)) 1000

    { new IDisposable with
        member _.Dispose() = JS.clearInterval interValid }

  start

let subscribe state = [ if state.Enabled then [ "timer" ], timer Tick ]

Program.mkProgram init update render
|> Program.withSubscription subscribe
|> Program.withReactSynchronous "elmish-app"
|> Program.run
