module App

open Elmish
open Elmish.HMR
open Fable.SimpleHttp
open Feliz

type Deferred<'t> =
  | HasNotStartedYet
  | InProgress
  | Resolved of 't

type AsyncOperationStatus<'t> =
  | Started
  | Finished of 't

type State =
  { LoremIpsum: Deferred<Result<string, string>> }

type Msg = LoadLoremIpsum of AsyncOperationStatus<Result<string, string>>

let init () = { LoremIpsum = HasNotStartedYet }, Cmd.ofMsg (LoadLoremIpsum Started)

module Cmd =
  let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
    let delayedCmd (dispatch: 'msg -> unit) : unit =
      async {
        let! msg = operation
        dispatch msg
      }
      |> Async.StartImmediate

    Cmd.ofEffect delayedCmd

let update msg state =
  match msg with
  | LoadLoremIpsum Started ->
    let nextState = { state with LoremIpsum = InProgress }

    let loadLoremIpsum =
      async {
        let! response = Http.request "/lorem-ipsum.txt" |> Http.method GET |> Http.send

        if response.statusCode = 200 then
          return LoadLoremIpsum(Finished(Ok response.responseText))
        else
          return LoadLoremIpsum(Finished(Error "Could not load the content"))
      }

    nextState, Cmd.fromAsync loadLoremIpsum
  | LoadLoremIpsum (Finished result) ->
    let nextState = { state with LoremIpsum = Resolved result }
    nextState, Cmd.none

let render (state: State) (_: Msg -> unit) =
  match state.LoremIpsum with
  | HasNotStartedYet -> Html.none
  | InProgress -> Html.div "Loading..."
  | Resolved (Ok content) ->
    Html.div [ prop.style [ style.color.green ]
               prop.text content ]
  | Resolved (Error errorMsg) ->
    Html.div [ prop.style [ style.color.red ]
               prop.text errorMsg ]

Program.mkProgram init update render |> Program.withReactSynchronous "elmish-app" |> Program.run
