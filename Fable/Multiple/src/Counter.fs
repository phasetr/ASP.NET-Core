[<RequireQualifiedAccess>]
module Multiple.Counter

open Feliz

type State = { Count: int }

type Msg =
  | Increment
  | Decrement

let init () = { Count = 0 }

let update (counterMsg: Msg) (counterState: State) =
  match counterMsg with
  | Increment ->
    { counterState with
        Count = counterState.Count + 1 }
  | Decrement ->
    { counterState with
        Count = counterState.Count - 1 }

let render (state: State) (dispatch: Msg -> unit) =
  Html.div
    [ Html.button [ prop.onClick (fun _ -> dispatch Increment); prop.text "Increment" ]
      Html.button [ prop.onClick (fun _ -> dispatch Decrement); prop.text "Decrement" ]
      Html.h1 state.Count ]
