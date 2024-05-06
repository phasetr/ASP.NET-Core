module Router

open Elmish.UrlParser
open Elmish.Navigation
open Fable.React.Props

type SessionRoute =
  | Settings
  | NewArticle
  | EditArticle of string
  | Logout

type Route =
  | Login
  | Register
  | Article of string
  | Articles
  | Profile of string
  | SessionRoute of SessionRoute

let pageParser: Parser<Route -> Route, Route> =
  oneOf
    [ map Article (s "article" </> str)
      map Articles top
      map Login (s "login")
      map Register (s "register")
      map (Settings |> SessionRoute) (s "settings")
      map (NewArticle |> SessionRoute) (s "editor")
      map (EditArticle >> SessionRoute) (s "editor" </> str)
      map Profile (s "profile" </> str)
      map (Logout |> SessionRoute) (s "logout") ]

let toHash route =
  match route with
  | Articles -> ""
  | Article slug -> $"article/%s{slug}"
  | Login -> "login"
  | Register -> "register"
  | SessionRoute Settings -> "settings"
  | SessionRoute NewArticle -> "editor"
  | Profile username -> $"profile/%s{username}"
  | SessionRoute Logout -> "logout"
  | SessionRoute(EditArticle slug) -> $"editor/%s{slug}"
  |> sprintf "#/%s"

let href = toHash >> Href

let modifyUrl route = route |> toHash |> Navigation.modifyUrl

let newUrl route = route |> toHash |> Navigation.newUrl
