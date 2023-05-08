# README

## データベースをPostgreSQLにする

```postgresql
drop schema public cascade;
create schema public;
```

`appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=user;Password=pass;Host=localhost;Port=5432;Database=mydb;"
  }
}
```

```shell

```shell
rm -rf Migrations && efm add Initialize && efd update
```
