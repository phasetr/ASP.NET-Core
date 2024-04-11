module App

open Browser.Dom

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
