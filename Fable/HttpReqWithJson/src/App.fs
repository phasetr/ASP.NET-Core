module App

open Elmish
open Elmish.React
open Fable.SimpleHttp
open Feliz
open Thoth.Json

type Product = { name: string; price: float }

type StoreInfo =
  { name: string
    since: int
    daysOpen: string list
    products: Product list }

type State =
  { StoreInfo: Deferred<Result<StoreInfo, string>> }

type Msg = LoadStoreInfo of AsyncOperationEvent<Result<string, string>>

let productDecoder: Decoder<Product> =
  Decode.object (fun get ->
    { name = get.Required.Field "name" Decode.string
      price = get.Required.Field "price" Decode.float })

let storeInfoDecoder: Decoder<StoreInfo> =
  Decode.object (fun get ->
    { name = get.Required.Field "name" Decode.string
      since = get.Required.Field "since" (Decode.map int Decode.int)
      daysOpen = get.Required.Field "daysOpen" (Decode.list Decode.string)
      products = get.Required.Field "products" (Decode.list productDecoder) })

let parseStoreInfo (json: string) : Result<StoreInfo, string> =
  Decode.Auto.fromString<StoreInfo> json

let init () = { StoreInfo = HasNotStartedYet }, Cmd.ofMsg (LoadStoreInfo Started)

let update (msg: Msg) (state: State) =
  match msg with
  | LoadStoreInfo Started ->
    let nextState = { state with StoreInfo = InProgress }

    let loadStoreInfo =
      async {
        // simulate delay
        do! Async.Sleep 1500
        let! statusCode, storeInfo = Http.get "/store.json"

        if statusCode = 200 then
          return LoadStoreInfo(Finished(Ok storeInfo))
        else
          return LoadStoreInfo(Finished(Error "Could not load the store information"))
      }

    nextState, Cmd.fromAsync loadStoreInfo

  | LoadStoreInfo (Finished (Ok storeInfoJson)) ->
    // Here, we are able to retrieve the JSON from the server
    // Now we try to parse thr JSON to a `StoreInfo` instance
    match parseStoreInfo storeInfoJson with
    | Ok storeInfo ->
      // JSON was parsed successfully into `StoreInfo`
      let nextState = { state with StoreInfo = Resolved(Ok storeInfo) }

      nextState, Cmd.none
    | Error error ->
      // JSON parsing failed here :/
      let nextState = { state with StoreInfo = Resolved(Error error) }

      nextState, Cmd.none
  | LoadStoreInfo (Finished (Error httpError)) ->
    let nextState = { state with StoreInfo = Resolved(Error httpError) }

    nextState, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  match state.StoreInfo with
  | HasNotStartedYet -> Html.none
  | InProgress -> Html.h1 "Loading..."
  | Resolved (Error errorMsg) ->
    Html.h1 [ prop.style [ style.color.red ]
              prop.text errorMsg ]
  | Resolved (Ok storeInfo) ->
    Html.div [ Html.h1 storeInfo.name
               Html.ul [ for product in storeInfo.products -> Html.li product.name ] ]

Program.mkProgram init update render |> Program.withReactSynchronous "elmish-app" |> Program.run
