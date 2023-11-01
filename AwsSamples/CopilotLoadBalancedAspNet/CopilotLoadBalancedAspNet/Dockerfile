FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CopilotLoadBalancedAspNet.csproj" \
  && dotnet build "CopilotLoadBalancedAspNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet tool restore \
  && dotnet restore "CopilotLoadBalancedAspNet.csproj" \
  && dotnet publish "CopilotLoadBalancedAspNet.csproj" -c Release -o /app/publish \
  && dotnet dotnet-ef migrations bundle -o /app/publish/bundle

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CopilotLoadBalancedAspNet.dll"]
