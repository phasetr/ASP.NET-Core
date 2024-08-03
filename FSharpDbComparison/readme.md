# README

- [English](readme.md)
- [日本語](readme_ja.md)

## Verification Environment

- `Apple M1 Pro`
- `.NET 8.0.204`

## Database Comparison: 2024-07-27

According to Wlaschin's [Domain Modeling Made Functional](https://tatsu-zine.com/books/domain-modeling-made-functional), `F#` doesn't typically use `ORMs`. However, [Dapper.FSharp](https://github.com/Dzoukr/Dapper.FSharp) is well-known and often mentioned. Regarding migrations, I gathered the following information from the `F#` Slack community and conducted my investigation.

Here, I record the information shared by the `Slack` community up to `2024-07-27`, as well as my observations on using `ORMs` and type providers for database access.

## `ORM`

- Summary article by `Compositional IT`: [SQL series wrap up](https://www.compositional-it.com/news-blog/sql-series-wrap-up/)

### `Dapper.FSharp`

It was more challenging than expected. In `Rider`, some parts of the code were highlighted, and I couldn't resolve the issues. For now, I'll leave it as it is for the basic implementation and will add details once I understand them better.

### `EF Core`

- [EFCore.FSharp](https://efcore.github.io/EFCore.FSharp/)
- GitHub, [GETTING_STARTED.md](https://github.com/efcore/EFCore.FSharp/blob/master/GETTING_STARTED.md)

It's possible to fully delegate database operations, including migrations, to `EF Core` as an external interface. One option is to switch from `F#` to `C#`. `EF Core` also includes `Identity`.

Using `EFCore.FSharp`, it's recommended to use [RecordHelper](https://efcore.github.io/EFCore.FSharp//How_Tos/Use_DbContextHelpers.html).

#### Reference: `EF Core` in `C#`

I want to write similar processes in [EFCoreCSharp](EFCoreCSharp) as those described in EFCoreCSharp.

#### Reference: Simple `EF Core` in `F#` (not `EFCore.FSharp`)

Refer to `efcore.fsx`. (The source was generated using `ChatGPT`.) It generates a `sqlite` database and some basic data. I couldn't set composite keys or relationships using `Fluent API` in `F#`, so I gave up after creating a basic sample.

### `SqlClient`

- [SqlClient](https://github.com/fsprojects/FSharp.Data.SqlClient)

It appears to be exclusively for `SQL Server`. Since I don't plan to use `SQL Server` at the moment, this is just a record.

### `SqlProvider`

- [GitHub](https://github.com/fsprojects/SQLProvider/)
- [Documentation Site](https://fsprojects.github.io/SQLProvider/core/general.html)

> When using database vendors other than SQL Server, Access, or ODBC, third-party drivers are required.

A concern noted in [this article](https://www.compositional-it.com/news-blog/full-orms-and-f/) is:

> The provider always requires a live connection to the database, so care is needed at various stages of development, such as `CI`.

However, there's a reference to an article offering a solution:

- [Structuring an F# project with SQL Type Provider on CI](https://medium.com/datarisk-io/structuring-an-f-project-with-sql-type-provider-on-ci-787a79d78699)

#### Using `MariaDB`

Refer to `sqlprovider-mariadb.fsx`. Ensure `MariaDB` is running via `Docker`. The script copies `MySqlConnector.dll` and loads it.

#### Using `PostgreSQL`

Refer to `sqlprovider-postgresql.fsx`. Ensure `PostgreSQL` is running via `Docker`. The script copies `Npgsql.dll` and loads it.

#### Using `SQLite`

Refer to `sqlprovider-sqlite.fsx`.

#### Using `SQL Server`

**Abandoned investigation due to malfunction**. The cause might be `Mac`.

Refer to `sqlprovider-sqlserver.fsx`. Ensure `SQL Server` is running via `Docker`. Abandoned due to the error `Microsoft.Data.SqlClient is not supported on this platform.` It might only work on `Windows`.

## Migration Tools

### `SSDT database projects (.sqlproj) for years alongside our SAFE Stack apps`

- [Using grate with SSDT Database Projects](https://www.compositional-it.com/news-blog/using-grate-with-ssdt-database-projects/)
- [Migration-based database development](https://www.compositional-it.com/news-blog/migration-based-database-development/)

Exclusively for `SQL Server`. The first impression is that I don't plan to use `SQL Server` much, so what to do?

### `grate`

Recommended in the above articles.

- [grate](https://erikbra.github.io/grate/)
- It was recommended over `Dbup` in the above articles.
- It also uses plain SQL scripts.
- If going plain, this might be the way to go.
- Install with `dotnet tool install --global grate` (or `--local`).
- Essentially, execute the `grate` command in the directory where `up` and `down` scripts are located.
  - Here, necessary files are collected in the `grate-*` directory, and the root directory can be specified with the `-f` option.
  - At the time of writing, `MariaDB` is used instead of `MySQL` due to errors with `MySQL`.
- [Command Options Explanation Page](https://erikbra.github.io/grate/configuration-options/)
  - `-o`: Where everything related to the migration is stored. All backups, all executed items, permission dumps, logs, etc.
  - `–env`: Specifies the environment to apply

#### `grate` Local Installation

- Install `grate` with the following commands:

```shell
dotnet new tool-manifest
dotnet tool install --local grate
```

#### Notes for Running `grate-mariadb`

- Start Docker.
- Connection string: `Server=localhost;Port=3306;Database=mydb;User Id=user;Password=pass;`
- Run migration

```shell
dotnet tool run grate \
  -c="Host=localhost;Port=5432;Database=mydb;Username=user;Password=pass" \
  -f grate-postgresql \
  --dbt postgresql
```

- Stop Docker.

#### Notes for Running `grate-sqlite`

- Run migration

```shell
dotnet tool run grate \
  -c="Data Source=grate-sqlite.db" \
  -f grate-sqlite \
  --dbt sqlite
```

#### Notes for Running grate-sqlserver

- Start Docker.
- Unconfirmed: The SQL in grate-sqlserver/init-db is supposed to create the database, but it might not be working. Create Database=mydb using appropriate means like connecting from Rider.
- Run migration

```shell
dotnet tool run grate \
  -c="Server=localhost,1433;Database=mydb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True" \
  -f grate-sqlserver \
  --dbt sqlserver
```

### `Dbup`

- [Dbup](https://dbup.readthedocs.io/en/latest/)

It appears to be a library used via Nuget rather than installed with `dotnet tool install`? 
It can also be used standalone. 
It also uses plain SQL scripts. 
No related code written yet.

### `Evolve`

- [Evolve](https://evolve-db.netlify.app/)
- Uses plain SQL scripts.

No related code written yet.

### `FluentMigrator`

- [Fluent migrations framework for .NET](https://fluentmigrator.github.io/)

>Fluent Migrator is a migration framework for .NET much like Ruby on Rails Migrations.

Actual code is written in C# to manage migrations. According to what I heard on Slack, it handles downgrades well.
