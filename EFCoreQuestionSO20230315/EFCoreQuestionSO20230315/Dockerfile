FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EFCoreQuestionSO20230315.csproj", "./"]
RUN dotnet restore "EFCoreQuestionSO20230315.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EFCoreQuestionSO20230315.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EFCoreQuestionSO20230315.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EFCoreQuestionSO20230315.dll"]
