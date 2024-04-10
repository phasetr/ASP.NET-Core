module App
open Browser.Dom

printfn "Hello from Fable"

let printMsgButton = document.getElementById "printMsg"

printMsgButton.onclick <- fun _ -> printfn "Button clicked!"
