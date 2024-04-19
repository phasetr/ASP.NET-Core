[<AutoOpen>]
module Multiple.ApplicationExtensions

open Elmish

module Cmd =
  let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
    let delayedCmd (dispatch: 'msg -> unit) : unit =
      async {
        let! msg = operation
        dispatch msg
      }
      |> Async.StartImmediate

    Cmd.ofEffect delayedCmd
