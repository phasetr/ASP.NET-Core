[<RequireQualifiedAccess>]
module Home

open Elmish
open Feliz

type State = { User: Api.User }

type Msg = | Logout

let init (user: Api.User) = { User = user }, Cmd.none

let update (msg: Msg) (state: State) : State * Cmd<Msg> =
  match msg with
  | Logout -> state, Cmd.none

let centered (children: ReactElement list) =
  Html.div [ prop.style [ style.margin.auto
                          style.textAlign.center
                          style.padding 20
                          style.width (length.percent 100) ]
             prop.children children ]

let render (state: State) (dispatch: Msg -> unit) =
  centered [ Html.h1 [ Html.strong (state.User.Username.ToUpper()) ]
             Html.button [ prop.className "button is-info"
                           prop.onClick (fun _ -> dispatch Logout)
                           prop.text "Logout" ] ]
