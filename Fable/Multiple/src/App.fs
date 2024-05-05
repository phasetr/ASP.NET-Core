[<RequireQualifiedAccess>]
module Multiple.App

open Feliz
open Elmish

[<RequireQualifiedAccess>]
type Page =
  | Counter of Counter.State
  | InputText of InputText.State

type State = { CurrentPage: Page }

type Msg =
  | CounterMsg of Counter.Msg
  | InputTextMsg of InputText.Msg
  | SwitchToCounter
  | SwitchToInputText

let init () =
  let counterState, counterCmd = Counter.init ()
  let initialState = { CurrentPage = Page.Counter counterState }
  let initialCmd = Cmd.map CounterMsg counterCmd

  initialState, initialCmd

let update (msg: Msg) (state: State) =
  match state.CurrentPage, msg with
  | Page.Counter counterState, CounterMsg counterMsg ->
    let counterState, counterCmd = Counter.update counterMsg counterState
    let nextState = { state with CurrentPage = Page.Counter counterState }
    let nextCmd = Cmd.map CounterMsg counterCmd
    nextState, nextCmd
  | Page.InputText inputTextState, InputTextMsg inputTextMsg ->
    let updatedInputText, inputTextCmd = InputText.update inputTextMsg inputTextState
    let nextState = { state with CurrentPage = Page.InputText updatedInputText }
    let nextCmd = Cmd.map InputTextMsg inputTextCmd
    nextState, nextCmd
  | _, SwitchToCounter ->
    let counterState, counterCmd = Counter.init ()
    let nextState = { state with CurrentPage = Page.Counter counterState }
    let nextCmd = Cmd.map CounterMsg counterCmd
    nextState, nextCmd
  | _, SwitchToInputText ->
    let inputTextState, inputTextCmd = InputText.init ()
    let nextState = { state with CurrentPage = Page.InputText inputTextState }
    let nextCmd = Cmd.map InputTextMsg inputTextCmd
    nextState, nextCmd
  | _, _ -> state, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  match state.CurrentPage with
  | Page.Counter counterState ->
    Html.div [ Html.button [ prop.text "Show Text Input"
                             prop.onClick (fun _ -> dispatch SwitchToInputText) ]
               Common.divider
               Counter.render counterState (CounterMsg >> dispatch) ]
  | Page.InputText inputTextState ->
    Html.div [ Html.button [ prop.text "Show counter"
                             prop.onClick (fun _ -> dispatch SwitchToCounter) ]
               Common.divider
               InputText.render inputTextState (InputTextMsg >> dispatch) ]
