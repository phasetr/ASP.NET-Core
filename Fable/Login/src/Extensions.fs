[<AutoOpen>]
module Extensions

open Elmish

type Deferred<'t> =
  | HasNotStartedYet
  | InProgress
  | Resolved of 't

type AsyncOperationStatus<'t> =
  | Started
  | Finished of 't

module Cmd =
  let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
    let delayedCmd (dispatch: 'msg -> unit) : unit =
      async {
        match! Async.Catch operation with
        | Choice1Of2 msg -> dispatch msg
        | Choice2Of2 error -> ignore ()
      }
      |> Async.StartImmediate

    Cmd.ofEffect delayedCmd

module Async =
  let map f computation =
    async {
      let! x = computation
      return f x
    }
