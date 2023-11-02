FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApiAuth0/WebApiAuth0.csproj", "WebApiAuth0/"]
RUN dotnet restore "WebApiAuth0/WebApiAuth0.csproj"
COPY . .
WORKDIR "/src/WebApiAuth0"
RUN dotnet build "WebApiAuth0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiAuth0.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiAuth0.dll"]
