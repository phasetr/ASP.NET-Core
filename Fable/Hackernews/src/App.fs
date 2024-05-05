module App

open Elmish
open Elmish.React
open Feliz
open Thoth.Json
open Fable.SimpleHttp

type HackerNewsItem =
  { id: int
    title: string
    itemType: string
    url: string option
    score: int
    time: int64 }

[<RequireQualifiedAccess>]
type Stories =
  | New
  | Best
  | Top
  | Job

type DeferredResult<'t> = Deferred<Result<'t, string>>

type DeferredStoryItem = DeferredResult<HackerNewsItem>

type State =
  { CurrentStories: Stories
    StoryItems: DeferredResult<Map<int, DeferredStoryItem>> }

type Msg =
  | ChangeStories of Stories
  | LoadStoryItems of AsyncOperationEvent<Result<int list, string>>
  | LoadedStoryItem of int * Result<HackerNewsItem, string>

let init () =
  { CurrentStories = Stories.New
    StoryItems = HasNotStartedYet },
  Cmd.ofMsg (LoadStoryItems Started)

let itemDecoder: Decoder<HackerNewsItem> =
  Decode.object (fun fields ->
    { id = fields.Required.At [ "id" ] Decode.int
      title = fields.Required.At [ "title" ] Decode.string
      itemType = fields.Required.At [ "type" ] Decode.string
      url = fields.Optional.At [ "url" ] Decode.string
      score = fields.Required.At [ "score" ] Decode.int
      time = fields.Required.At [ "time" ] Decode.int64 })

let storiesEndpoint stories =
  let fromBaseUrl = sprintf "https://hacker-news.firebaseio.com/v0/%sstories.json"

  match stories with
  | Stories.Best -> fromBaseUrl "best"
  | Stories.Top -> fromBaseUrl "top"
  | Stories.New -> fromBaseUrl "new"
  | Stories.Job -> fromBaseUrl "job"

let (|HttpOk|HttpError|) status =
  match status with
  | 200 -> HttpOk
  | _ -> HttpError

let rnd = System.Random()

let loadStoryItem (id: int) =
  async {
    // simulate high network latency
    do! Async.Sleep(rnd.Next(1000, 3000))
    let endpoint = $"https://hacker-news.firebaseio.com/v0/item/%d{id}.json"
    let! status, responseText = Http.get endpoint

    match status with
    | HttpOk ->
      match Decode.fromString itemDecoder responseText with
      | Ok storyItem -> return LoadedStoryItem(id, Ok storyItem)
      | Error parseError -> return LoadedStoryItem(id, Error parseError)
    | HttpError -> return LoadedStoryItem(id, Error("Http error while loading " + string id))
  }

let loadStoryItems stories =
  async {
    let endpoint = storiesEndpoint stories
    let! status, responseText = Http.get endpoint

    match status with
    | HttpOk ->
      // parse the response text as a list of IDs (ints)
      let storyIds = Decode.fromString (Decode.list Decode.int) responseText

      match storyIds with
      | Ok storyIds ->
        let firstTenStories = List.truncate 10 storyIds
        return LoadStoryItems(Finished(Ok firstTenStories))
      | Error parseError -> return LoadStoryItems(Finished(Error parseError))

    | HttpError ->
      // non-OK response finishes with an error
      return LoadStoryItems(Finished(Error "Could not load the IDs of the story items."))
  }

let startLoading (state: State) = { state with StoryItems = InProgress }

let update (msg: Msg) (state: State) =
  match msg with
  | ChangeStories stories ->
    let nextState = { startLoading state with CurrentStories = stories }
    let nextCmd = Cmd.fromAsync (loadStoryItems stories)
    nextState, nextCmd
  | LoadStoryItems Started ->
    let nextState = startLoading state
    let nextCmd = Cmd.fromAsync (loadStoryItems state.CurrentStories)
    nextState, nextCmd
  | LoadStoryItems (Finished (Ok storyIds)) ->
    // initialize the story IDs
    let storiesMap = Map.ofList [ for id in storyIds -> id, Deferred.InProgress ]
    let nextState = { state with StoryItems = Resolved(Ok storiesMap) }
    nextState, Cmd.batch [ for id in storyIds -> Cmd.fromAsync (loadStoryItem id) ]
  | LoadStoryItems (Finished (Error error)) ->
    let nextState = { state with StoryItems = Resolved(Error error) }
    nextState, Cmd.none
  | LoadedStoryItem (itemId, Ok item) ->
    match state.StoryItems with
    | Resolved (Ok storiesMap) ->
      let modifiedStoriesMap = storiesMap |> Map.remove itemId |> Map.add itemId (Resolved(Ok item))
      let nextState = { state with StoryItems = Resolved(Ok modifiedStoriesMap) }
      nextState, Cmd.none
    | _ -> state, Cmd.none
  | LoadedStoryItem (itemId, Error error) ->
    match state.StoryItems with
    | Resolved (Ok storiesMap) ->
      let modifiedStoriesMap =
        storiesMap |> Map.remove itemId |> Map.add itemId (Resolved(Error error))

      let nextState = { state with StoryItems = Resolved(Ok modifiedStoriesMap) }
      nextState, Cmd.none
    | _ -> state, Cmd.none

let storiesName =
  function
  | Stories.New -> "New"
  | Stories.Best -> "Best"
  | Stories.Job -> "Job"
  | Stories.Top -> "Top"

let renderTab currentStories stories dispatch =
  Html.li [ prop.className [ if currentStories = stories then "is-active" ]
            prop.onClick (fun _ ->
              if (currentStories <> stories) then dispatch (ChangeStories stories))
            prop.children [ Html.a [ Html.span (storiesName stories) ] ] ]

let stories = [ Stories.New; Stories.Top; Stories.Best; Stories.Job ]

let renderTabs currentStories dispatch =
  Html.div [ prop.className [ "tabs"
                              "is-toggle"
                              "is-fullwidth" ]
             prop.children [ Html.ul [ for story in stories ->
                                         renderTab currentStories story dispatch ] ] ]

let renderError (errorMsg: string) =
  Html.h1 [ prop.style [ style.color.red ]
            prop.text errorMsg ]

let div (classes: string list) (children: ReactElement list) =
  Html.div [ prop.className classes
             prop.children children ]

let spinner =
  Html.div [ prop.style [ style.textAlign.center
                          style.marginTop 20 ]
             prop.children [ Html.i [ prop.className "fa fa-cog fa-spin fa-2x" ] ] ]

let renderItemContent (item: HackerNewsItem) =
  Html.div [ div [ "columns"; "is-mobile" ] [
               div [ "column"; "is-narrow" ] [
                 Html.div [ prop.className [ "icon" ]
                            prop.style [ style.marginLeft 20 ]
                            prop.children [ Html.i [ prop.className "fa fa-poll fa-2x" ]
                                            Html.span [ prop.style [ style.marginLeft 10
                                                                     style.marginRight 10 ]
                                                        prop.text item.score ] ] ]
               ]
               div [ "column" ] [
                 match item.url with
                 | Some url ->
                   Html.a [ prop.style [ style.textDecoration.underline ]
                            prop.target.blank
                            prop.href url
                            prop.text item.title ]
                 | None -> Html.p item.title
               ]
             ] ]

let renderStoryItem (itemId: int) storyItem =
  let renderedItem =
    match storyItem with
    | HasNotStartedYet -> Html.none
    | InProgress -> spinner
    | Resolved (Error error) -> renderError error
    | Resolved (Ok storyItem) -> renderItemContent storyItem

  Html.div [ prop.key itemId
             prop.className "box"
             prop.style [ style.marginTop 15
                          style.marginBottom 15 ]
             prop.children [ renderedItem ] ]

let renderStories items =
  match items with
  | HasNotStartedYet -> Html.none
  | InProgress -> spinner
  | Resolved (Error errorMsg) -> renderError errorMsg
  | Resolved (Ok items) ->
    items
    |> Map.toList
    |> List.sortByDescending (fun (_, storyItem) ->
      match storyItem with
      | Resolved (Ok item) -> item.time
      | _ -> 0)
    |> List.map (fun (id, storyItem) -> renderStoryItem id storyItem)
    |> Html.div

let title =
  Html.h1 [ prop.className "title"
            prop.text "Elmish Hacker News" ]

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [ prop.style [ style.padding 20 ]
             prop.children [ title
                             renderTabs state.CurrentStories dispatch
                             renderStories state.StoryItems ] ]

Program.mkProgram init update render |> Program.withReactSynchronous "elmish-app" |> Program.run
