module Multiple.Main

open Elmish
open Elmish.React

Program.mkSimple App.init App.update App.render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
