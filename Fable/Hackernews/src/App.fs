module App

open Elmish
open Elmish.React
open Feliz
open Thoth.Json
open Fable.SimpleHttp

type HackerNewsItem =
  { id: int
    title: string
    url: string option
    score: int }

[<RequireQualifiedAccess>]
type Stories =
  | New
  | Best
  | Top
  | Job

type State =
  { CurrentStories: Stories
    StoryItems: Deferred<Result<HackerNewsItem list, string>> }

type Msg =
  | ChangeStories of Stories
  | LoadStoryItems of AsyncOperationEvent<Result<HackerNewsItem list, string>>

let init () =
  { CurrentStories = Stories.New
    StoryItems = HasNotStartedYet },
  Cmd.ofMsg (LoadStoryItems Started)

let itemDecoder: Decoder<HackerNewsItem> =
  Decode.object (fun fields ->
    { id = fields.Required.At [ "id" ] Decode.int
      title = fields.Required.At [ "title" ] Decode.string
      url = fields.Optional.At [ "url" ] Decode.string
      score = fields.Required.At [ "score" ] Decode.int })

let storiesEndpoint stories =
  let fromBaseUrl = sprintf "https://hacker-news.firebaseio.com/v0/%sstories.json"

  match stories with
  | Stories.Best -> fromBaseUrl "best"
  | Stories.Top -> fromBaseUrl "top"
  | Stories.New -> fromBaseUrl "new"
  | Stories.Job -> fromBaseUrl "job"

let loadStoryItem (id: int) =
  async {
    let endpoint = $"https://hacker-news.firebaseio.com/v0/item/%d{id}.json"
    let! status, responseText = Http.get endpoint

    match status with
    | 200 ->
      match Decode.fromString itemDecoder responseText with
      | Ok storyItem -> return Some storyItem
      | Error _ -> return None
    | _ -> return None
  }

let loadStoryItems stories =
  async {
    let endpoint = storiesEndpoint stories
    let! status, responseText = Http.get endpoint

    match status with
    | 200 ->
      // parse the response text as a list of IDs (ints)
      let storyIds = Decode.fromString (Decode.list Decode.int) responseText

      match storyIds with
      | Ok storyIds ->
        // take the first 10 IDs
        // load the item from each ID in parallel
        // aggregate the results into a single list
        let! storyItems =
          storyIds
          |> List.truncate 10
          |> List.map loadStoryItem
          |> Async.Parallel
          |> Async.map (List.ofArray >> List.choose id)

        return LoadStoryItems(Finished(Ok storyItems))

      | Error errorMsg ->
        // could not parse the array of story ID's
        return LoadStoryItems(Finished(Error errorMsg))
    | _ ->
      // non-OK response goes finishes with an error
      return LoadStoryItems(Finished(Error responseText))
  }

let update (msg: Msg) (state: State) =
  match msg with
  | ChangeStories stories ->
    let nextState =
      { state with
          StoryItems = InProgress
          CurrentStories = stories }

    let nextCmd = Cmd.fromAsync (loadStoryItems stories)
    nextState, nextCmd
  | LoadStoryItems Started ->
    let nextState = { state with StoryItems = InProgress }
    let nextCmd = Cmd.fromAsync (loadStoryItems state.CurrentStories)
    nextState, nextCmd
  | LoadStoryItems(Finished items) ->
    let nextState =
      { state with
          StoryItems = Resolved items }

    nextState, Cmd.none

let storiesName =
  function
  | Stories.New -> "New"
  | Stories.Best -> "Best"
  | Stories.Job -> "Job"
  | Stories.Top -> "Top"

let storyCategories = [ Stories.New; Stories.Top; Stories.Best; Stories.Job ]

let renderTabs selectedStories dispatch =
  let switchStories stories =
    if selectedStories <> stories then
      dispatch (ChangeStories stories)

  Html.div
    [ prop.className [ "tabs"; "is-toggle"; "is-fullwidth" ]
      prop.children
        [ Html.ul
            [ for stories in storyCategories ->
                Html.li
                  [ prop.className
                      [ if selectedStories = stories then
                          "is-active" ]
                    prop.onClick (fun _ -> switchStories stories)
                    prop.children [ Html.a [ Html.span (storiesName stories) ] ] ] ] ] ]

let renderError (errorMsg: string) = Html.h1 [ prop.style [ style.color.red ]; prop.text errorMsg ]

let div (classes: string list) (children: ReactElement list) =
  Html.div [ prop.className classes; prop.children children ]

let renderItem item =
  Html.div
    [ prop.className "box"
      prop.style [ style.marginTop 15; style.marginBottom 15 ]
      prop.children
        [ div
            [ "columns"; "is-mobile" ]
            [ div
                [ "column"; "is-narrow" ]
                [ Html.div
                    [ prop.className [ "icon" ]
                      prop.style [ style.marginLeft 20 ]
                      prop.children
                        [ Html.i [ prop.className "fa fa-poll fa-2x" ]
                          Html.span [ prop.style [ style.marginLeft 10; style.marginRight 10 ]; prop.text item.score ] ] ] ]

              div
                [ "column" ]
                [ match item.url with
                  | Some url ->
                    Html.a
                      [ prop.style [ style.textDecoration.underline ]
                        prop.target.blank
                        prop.href url
                        prop.text item.title ]

                  | None -> Html.p item.title ] ] ] ]

let spinner =
  Html.div
    [ prop.style [ style.textAlign.center; style.marginTop 20 ]
      prop.children [ Html.i [ prop.className "fa fa-cog fa-spin fa-2x" ] ] ]

let renderStories items =
  match items with
  | HasNotStartedYet -> Html.none
  | InProgress -> spinner
  | Resolved(Error errorMsg) -> renderError errorMsg
  | Resolved(Ok items) -> React.fragment [ for item in items -> renderItem item ]

let render (state: State) (dispatch: Msg -> unit) =
  Html.div
    [ prop.style [ style.padding 20 ]
      prop.children
        [ Html.h1 [ prop.className "title"; prop.text "Elmish Hacker News" ]

          renderTabs state.CurrentStories dispatch
          renderStories state.StoryItems ] ]

Program.mkProgram init update render |> Program.withReactSynchronous "elmish-app" |> Program.run
