FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WebApi.csproj" \
  && dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN export PATH="$PATH:/root/.dotnet/tools" \
  && dotnet tool install --global dotnet-ef --version 6.0.18 \
  && dotnet publish "WebApi.csproj" -c Release -o /app/publish
  # && dotnet ef migrations bundle -o /app/publish/bundle --verbose

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
