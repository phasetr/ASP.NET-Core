[<RequireQualifiedAccess>]
module Multiple.App

open Feliz
open Elmish

[<RequireQualifiedAccess>]
type Page =
  | Counter
  | InputText

type State =
  { Counter: Counter.State
    InputText: InputText.State
    CurrentPage: Page }

type Msg =
  | CounterMsg of Counter.Msg
  | InputTextMsg of InputText.Msg
  | SwitchPage of Page

let init () =
  let counterState, counterCmd = Counter.init ()
  let inputTextState, inputTextCmd = InputText.init ()

  let initialState =
    { Counter = counterState
      InputText = inputTextState
      CurrentPage = Page.Counter }

  let initialCmd = Cmd.batch [ Cmd.map CounterMsg counterCmd; Cmd.map InputTextMsg inputTextCmd ]

  initialState, initialCmd

let update (msg: Msg) (state: State) =
  match msg with
  | CounterMsg counterMsg ->
    let updatedCounter, counterCmd = Counter.update counterMsg state.Counter
    { state with Counter = updatedCounter }, Cmd.map CounterMsg counterCmd
  | InputTextMsg inputTextMsg ->
    let updatedInputText, inputTextCmd = InputText.update inputTextMsg state.InputText

    { state with
        InputText = updatedInputText },
    Cmd.map InputTextMsg inputTextCmd
  | SwitchPage page -> { state with CurrentPage = page }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  match state.CurrentPage with
  | Page.Counter ->
    Html.div
      [ Html.button [ prop.text "Show Text Input"; prop.onClick (fun _ -> dispatch (SwitchPage Page.InputText)) ]
        Common.divider
        Counter.render state.Counter (CounterMsg >> dispatch) ]
  | Page.InputText ->
    Html.div
      [ Html.button [ prop.text "Show counter"; prop.onClick (fun _ -> dispatch (SwitchPage Page.Counter)) ]
        Common.divider
        InputText.render state.InputText (InputTextMsg >> dispatch) ]
