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
type Validated<'t> = { Raw: string; Parsed: Option<'t> }

module Validated =
  let createEmpty () : Validated<_> = { Raw = ""; Parsed = None }
  let success raw value : Validated<_> = { Raw = raw; Parsed = Some value }
  let failure raw : Validated<_> = { Raw = raw; Parsed = None }

let tryParseInt (input: string) : Validated<int> =
  try
    Validated.success input (int input)
  with
  | _ -> Validated.failure input

let validatedTextColor validated =
  match validated.Parsed with
  | Some _ -> color.green
  | None -> color.crimson

type State =
  { Count: int
    TextInput: string
    NumberInput: Validated<int>
    Capitalized: bool }

type Msg =
  | Increment
  | Decrement
  | SetNumberInput of Validated<int>
  | SetTextInput of string
  | SetCapitalized of bool

let init () =
  { Count = 0
    NumberInput = Validated.createEmpty ()
    TextInput = ""
    Capitalized = false }

let update (msg: Msg) (state: State) : State =
  match msg with
  | Increment -> { state with Count = state.Count + 1 }
  | Decrement -> { state with Count = state.Count - 1 }
  | SetNumberInput numberInput -> { state with NumberInput = numberInput }
  | SetTextInput textInput -> { state with TextInput = textInput }
  | SetCapitalized value -> { state with Capitalized = value }

let render (state: State) (dispatch: Msg -> unit) =
  let oddOrEvenMessage =
    Html.h1 [ prop.style [ if state.Count < 0 then style.display.none else style.display.block ]
              prop.text (if state.Count % 2 = 0 then "Count is even" else "Count is odd") ]

  Html.div [ Html.div [ prop.style [ style.padding 20 ]
                        prop.children [ Html.div [ Html.input [ prop.valueOrDefault state.TextInput
                                                                prop.onChange (
                                                                  SetTextInput >> dispatch
                                                                ) ]
                                                   Html.span state.TextInput ]

                                        Html.div [ Html.label [ prop.htmlFor "checkbox-capitalized"
                                                                prop.text "Capitalized" ]

                                                   Html.input [ prop.style [ style.margin 5 ]
                                                                prop.id "checkbox-capitalized"
                                                                prop.type'.checkbox
                                                                prop.isChecked state.Capitalized
                                                                prop.onChange (
                                                                  SetCapitalized >> dispatch
                                                                ) ]

                                                   Html.span (
                                                     if state.Capitalized then
                                                       state.TextInput.ToUpper()
                                                     else
                                                       state.TextInput
                                                   ) ]

                                        Html.input [ prop.type'.number
                                                     prop.valueOrDefault state.NumberInput.Raw
                                                     prop.onChange (
                                                       tryParseInt >> SetNumberInput >> dispatch
                                                     ) ]

                                        Html.h2 [ prop.style [ style.color (
                                                                 validatedTextColor
                                                                   state.NumberInput
                                                               ) ]
                                                  prop.text state.NumberInput.Raw ] ] ]
             Html.button [ prop.onClick (fun _ -> dispatch Increment)
                           prop.text "+" ]
             Html.button [ prop.onClick (fun _ -> dispatch Decrement)
                           prop.text "-" ]
             Html.div state.Count
             oddOrEvenMessage ]

Program.mkSimple init update render |> Program.withReactSynchronous "elmish-app" |> Program.run
