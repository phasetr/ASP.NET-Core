module Api

open Thoth.Json
open Fable.RemoteData
open Fable.SimpleHttp
open Types

let private baseUrl = "https://conduit.productionready.io/api/"

let private badRequestErrorDecoder str =
  str
  |> Decode.at
    [ "errors" ]
    (Decode.dict (Decode.list Decode.string)
     |> Decode.andThen (
       Map.toList
       >> List.collect (fun (key, errors) ->
         List.map (sprintf "%s %s" key) errors)
       >> Decode.succeed
     ))
// The errors are returned as a key/pair value of string * string list
// So converting all errors as just a simple string list

let private makeRequest method url decoder session body =
  async {
    let request = Http.request url |> Http.method method

    let request =
      session
      |> Option.map (fun s ->
        Http.header (Headers.authorization <| $"Token %s{s.Token}") request)
      |> Option.defaultValue request

    let request =
      body
      |> Option.map (fun b ->
        Http.content (BodyContent.Text b) request
        |> Http.header (Headers.contentType "application/json"))
      |> Option.defaultValue request

    let! response = Http.send request

    match response.statusCode with
    | 200 ->
      let decodedValue = Decode.fromString decoder response.responseText

      match decodedValue with
      | Ok value -> return Success value
      | Error e -> return Failure [ e ]
    | 422 ->
      let decodedErrors =
        Decode.fromString badRequestErrorDecoder response.responseText

      match decodedErrors with
      | Ok errors -> return Failure errors
      | Error e -> return Failure [ e ]
    | _ -> return Failure [ response.responseText ]
  }


let private safeGet url decoder session =
  makeRequest GET url decoder (Some session) None

let private safeDelete url decoder session =
  makeRequest DELETE url decoder (Some session) None

let private safeChange method (body: JsonValue) url decoder session =
  Some(Encode.toString 0 body) |> makeRequest method url decoder (Some session)


let private safePut url decoder session (body: JsonValue) =
  safeChange PUT body url decoder session


let private safePost url decoder session (body: JsonValue) =
  safeChange POST body url decoder session


let private get url decoder = makeRequest GET url decoder None None


let private post url decoder body =
  Some(Encode.toString 0 body) |> makeRequest POST url decoder None

module Articles =
  let articlesBaseUrl = $"%s{baseUrl}articles/"

  let fetchArticlesWithTag (payload: {| Tag: Tag; Offset: int |}) =
    let (Tag tag) = payload.Tag

    let url =
      $"%s{articlesBaseUrl}?tag=%s{tag}&limit=10&offset=%i{payload.Offset}"

    get url Article.ArticlesList.Decoder

  let fetchArticles offset =
    let url = $"%s{articlesBaseUrl}?limit=10&offset=%i{offset}"
    get url Article.ArticlesList.Decoder

  let fetchArticle slug =
    let url = $"%s{articlesBaseUrl}/%s{slug}"
    get url (Decode.field "article" FullArticle.Decoder)

  let fetchArticleWithSession (payload: {| Session: Session; Slug: string |}) =
    let url = $"%s{articlesBaseUrl}/%s{payload.Slug}"
    safeGet url (Decode.field "article" FullArticle.Decoder) payload.Session

  let fetchFeed (payload: {| Session: Session; Offset: int |}) =
    let url = $"%s{articlesBaseUrl}feed?limit=10&offset=%i{payload.Offset}"
    safeGet url Article.ArticlesList.Decoder payload.Session

  let fetchComments slug =
    let url = $"%s{articlesBaseUrl}/%s{slug}/comments"
    get url Comment.DecoderList

  let createArticle session (article: Article.ValidatedArticle) =
    Article.validatedToJson article
    |> safePost
      articlesBaseUrl
      (Decode.field "article" FullArticle.Decoder)
      session

  let updateArticle session (slug, article: Article.ValidatedArticle) =
    let url = $"%s{articlesBaseUrl}/%s{slug}"

    Article.validatedToJson article
    |> safePut url (Decode.field "article" FullArticle.Decoder) session

  let createComment
    (payload:
      {| Session: Session
         Slug: string
         CommentBody: string |})
    =
    let url = $"%s{articlesBaseUrl}/%s{payload.Slug}/comments"
    let comment = Encode.object [ ("body", Encode.string payload.CommentBody) ]

    safePost
      url
      (Decode.field "comment" Comment.Decoder)
      payload.Session
      {| comment = comment |}

  let favoriteArticle
    (payload:
      {| Session: Session
         Article: FullArticle |})
    =
    let url = $"%s{articlesBaseUrl}/%s{payload.Article.Slug}/favorite"
    safePost url (Decode.field "article" FullArticle.Decoder) payload.Session ""

  let unfavorArticle
    (payload:
      {| Session: Session
         Article: FullArticle |})
    =
    let url = $"%s{articlesBaseUrl}/%s{payload.Article.Slug}/favorite"
    safeDelete url (Decode.field "article" FullArticle.Decoder) payload.Session

  let fetchArticlesFromAuthor author =
    let url = $"%s{articlesBaseUrl}?author=%s{author}&limit=10&offset={0}"
    get url Article.ArticlesList.Decoder

  let deleteArticle (payload: {| Session: Session; Slug: string |}) =
    let url = $"%s{articlesBaseUrl}/%s{payload.Slug}"
    safeDelete url (Decode.succeed ()) payload.Session

  let fetchFavoriteArticles (author: Author) =
    let url =
      $"%s{articlesBaseUrl}?favorited=%s{author.Username}&limit=10&offset={0}"

    get url Article.ArticlesList.Decoder

module Tags =
  let fetchTags () =
    let url = $"%s{baseUrl}tags"
    get url Tag.ListDecoder

module Users =
  let usersBaseUrl = $"%s{baseUrl}users/"

  let createUser
    (createUser:
      {| username: string
         email: string
         password: string |})
    =
    post
      usersBaseUrl
      (Decode.field "user" Session.Decoder)
      {| user = createUser |}

  let login (credentials: {| email: string; password: string |}) =
    let url = $"%s{usersBaseUrl}login/"
    post url (Decode.field "user" Session.Decoder) {| user = credentials |}

  let fetchUserWithDecoder decoder session =
    let url = $"%s{baseUrl}user/"
    safeGet url (Decode.field "user" decoder) session

  let fetchUser session = fetchUserWithDecoder User.Decoder session

  let updateUser session (validatedUser: User.ValidatedUser, password) =
    let url = $"%s{baseUrl}user/"

    User.validatedToJsonValue validatedUser password
    |> safePut url (Decode.field "user" User.Decoder) session

module Profiles =
  let fetchProfile username =
    let url = $"%s{baseUrl}profiles/%s{username}/"
    get url (Decode.field "profile" Author.Decoder)

  let createFollower (payload: {| Session: Session; Author: Author |}) =
    let url = $"%s{baseUrl}profiles/%s{payload.Author.Username}/follow"
    safePost url (Decode.field "profile" Author.Decoder) payload.Session ""

  let deleteFollower (payload: {| Session: Session; Author: Author |}) =
    let url = $"%s{baseUrl}profiles/%s{payload.Author.Username}/follow"
    safeDelete url (Decode.field "profile" Author.Decoder) payload.Session
