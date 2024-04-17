namespace Common.Constants

// Roles enum を定義
module Authorization =
  type Roles =
    | Administrator
    | Moderator
    | User

  let DefaultUsername = "user"
  let DefaultEmail = "user@phasetr.com"
  let DefaultPassword = "Pa$$w0rd."
  let DefaultRole = Roles.User
  let JwtAccessTokenName = "accessToken"
