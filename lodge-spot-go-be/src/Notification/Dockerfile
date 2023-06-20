FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Notification/LodgeSpotGo.Notifications.Api/LodgeSpotGo.Notifications.Api.csproj", "Notification/LodgeSpotGo.Notifications.Api/"]
RUN dotnet restore "Notification/LodgeSpotGo.Notifications.Api/LodgeSpotGo.Notifications.Api.csproj"
COPY . .
WORKDIR "/src/Notification/LodgeSpotGo.Notifications.Api"
RUN dotnet build "LodgeSpotGo.Notifications.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.Notifications.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.Notifications.Api.dll"]
