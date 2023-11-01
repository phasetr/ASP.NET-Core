FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet tool restore \
    && dotnet restore "CopilotRequestDriven.csproj" \
    && dotnet build "CopilotRequestDriven.csproj" -c Release -o /app/build

FROM build AS publish
ENV ASPNETCORE_ENVIRONMENT Staging
RUN dotnet tool restore \
    && dotnet restore "CopilotRequestDriven.csproj" \
    && dotnet publish "CopilotRequestDriven.csproj" -c Release -o /app/publish \
    && dotnet dotnet-ef migrations bundle -o /app/publish/bundle

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Staging
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CopilotRequestDriven.dll"]
