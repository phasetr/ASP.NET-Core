FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClientAuth0/ClientAuth0.csproj", "ClientAuth0/"]
RUN dotnet restore "ClientAuth0/ClientAuth0.csproj"
COPY . .
WORKDIR "/src/ClientAuth0"
RUN dotnet build "ClientAuth0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientAuth0.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientAuth0.dll"]
