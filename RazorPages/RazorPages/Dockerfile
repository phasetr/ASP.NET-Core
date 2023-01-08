FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RazorPages/RazorPages.csproj", "RazorPages/"]
RUN dotnet restore "RazorPages/RazorPages.csproj"
COPY . .
WORKDIR "/src/RazorPages"
RUN dotnet build "RazorPages.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RazorPages.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RazorPages.dll"]
