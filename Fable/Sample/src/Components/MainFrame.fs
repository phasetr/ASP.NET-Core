module Sample.Components

open Feliz

let mainFrame (children: Fable.React.ReactElement seq) =
  Html.main
    [ prop.className "py-8 px-4 sm:px-6 lg:px-8"; prop.children children ]

let frameHeader (text: string) =
  Html.h1 [ prop.className "text-3xl font-bold mb-4"; prop.text text ]
