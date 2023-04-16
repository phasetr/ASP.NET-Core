FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/BlazorWasmHosted.Server.csproj", "Server/"]
COPY ["Client/BlazorWasmHosted.Client.csproj", "Client/"]
COPY ["Shared/BlazorWasmHosted.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/BlazorWasmHosted.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "BlazorWasmHosted.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorWasmHosted.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorWasmHosted.Server.dll"]
