[<RequireQualifiedAccess>]
module Multiple.App

open Feliz
open Multiple.Common
open Multiple

[<RequireQualifiedAccess>]
type Page =
  | Counter
  | TextInput

type State =
  { Counter: Counter.State
    InputText: InputText.State
    CurrentPage: Page }

type Msg =
  | CounterMsg of Counter.Msg
  | InputTextMsg of InputText.Msg
  | SwitchPage of Page

let init () =
  { Counter = Counter.init ()
    InputText = InputText.init ()
    CurrentPage = Page.Counter }

let update (msg: Msg) (state: State) : State =
  match msg with
  | CounterMsg counterMsg ->
    let updatedCounter = Counter.update counterMsg state.Counter
    { state with Counter = updatedCounter }

  | InputTextMsg inputTextMsg ->
    let updatedInputText = InputText.update inputTextMsg state.InputText

    { state with
        InputText = updatedInputText }

  | SwitchPage page -> { state with CurrentPage = page }

let render (state: State) (dispatch: Msg -> unit) =
  match state.CurrentPage with
  | Page.Counter ->
    Html.div
      [ Html.button [ prop.text "Show Text Input"; prop.onClick (fun _ -> dispatch (SwitchPage Page.TextInput)) ]
        divider
        Counter.render state.Counter (CounterMsg >> dispatch) ]
  | Page.TextInput ->
    Html.div
      [ Html.button [ prop.text "Show counter"; prop.onClick (fun _ -> dispatch (SwitchPage Page.Counter)) ]
        divider
        InputText.render state.InputText (InputTextMsg >> dispatch) ]
