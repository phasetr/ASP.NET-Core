module App

open Browser.Dom
open Elmish
open Elmish.HMR
open Feliz

printfn "Hello from Fable"

let increaseButton = document.getElementById "increase"
let decreaseButton = document.getElementById "decrease"
let countViewer = document.getElementById "countViewer"

let mutable currentCount = 0
let rnd = System.Random()

let runAfter (ms: int) callback =
  async {
    do! Async.Sleep ms
    do callback ()
  }
  |> Async.StartImmediate

let increaseDelayed = document.getElementById "increaseDelayed"

increaseButton.onclick <-
  fun _ ->
    currentCount <- currentCount + rnd.Next(5, 10)
    countViewer.innerHTML <- $"Count is at %d{currentCount}"

decreaseButton.onclick <-
  fun _ ->
    currentCount <- currentCount - rnd.Next(5, 10)
    countViewer.innerHTML <- $"Count is at %d{currentCount}"

increaseDelayed.onclick <-
  fun _ ->
    runAfter 1000 (fun () ->
      currentCount <- currentCount + rnd.Next(5, 10)
      countViewer.innerHTML <- $"Count is at %d{currentCount}")

countViewer.innerHTML <- $"Count is at %d{currentCount}"

let printMsgButton = document.getElementById "printMsg"

printMsgButton.onclick <- fun _ -> printfn "Button clicked!"

// Elmish
type State = { Count: int }

type Msg =
  | Increment
  | Decrement

let init () = { Count = 0 }

let update (msg: Msg) (state: State) : State =
  match msg with
  | Increment -> { state with Count = state.Count + 1 }
  | Decrement -> { state with Count = state.Count - 1 }

let render (state: State) (dispatch: Msg -> unit) =
  Html.div
    [ Html.button [ prop.onClick (fun _ -> dispatch Increment); prop.text "Increment" ]
      Html.button [ prop.onClick (fun _ -> dispatch Decrement); prop.text "Decrement" ]
      Html.h1 state.Count ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
