FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Serilog/Serilog.csproj", "Serilog/"]
RUN dotnet restore "Serilog/Serilog.csproj"
COPY . .
WORKDIR "/src/Serilog"
RUN dotnet build "Serilog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Serilog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Serilog.dll"]
