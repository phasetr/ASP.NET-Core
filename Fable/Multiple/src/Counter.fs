[<RequireQualifiedAccess>]
module Multiple.Counter

open Feliz
open Elmish

type State = { Count: int }

type Msg =
  | Increment
  | Decrement
  | IncrementDelayed

let init () = { Count = 0 }, Cmd.none

let update (counterMsg: Msg) (counterState: State) =
  match counterMsg with
  | Increment -> { counterState with Count = counterState.Count + 1 }, Cmd.none
  | Decrement -> { counterState with Count = counterState.Count - 1 }, Cmd.none
  | IncrementDelayed ->
    let delayedIncrement =
      async {
        do! Async.Sleep 1000
        return Increment
      }

    counterState, Cmd.fromAsync delayedIncrement

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [ Html.button [ prop.onClick (fun _ -> dispatch Increment)
                           prop.text "Increment" ]
             Html.button [ prop.onClick (fun _ -> dispatch Decrement)
                           prop.text "Decrement" ]
             Html.button [ prop.onClick (fun _ -> dispatch IncrementDelayed)
                           prop.text "Increment Delayed" ]
             Html.h1 state.Count ]
