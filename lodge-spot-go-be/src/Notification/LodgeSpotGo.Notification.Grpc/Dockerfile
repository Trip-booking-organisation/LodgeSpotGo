FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Notification/LodgeSpotGo.Notification.Grpc/LodgeSpotGo.Notification.Grpc.csproj", "src/Notification/LodgeSpotGo.Notification.Grpc/"]
RUN dotnet restore "src/Notification/LodgeSpotGo.Notification.Grpc/LodgeSpotGo.Notification.Grpc.csproj"
COPY . .
WORKDIR "/src/src/Notification/LodgeSpotGo.Notification.Grpc"
RUN dotnet build "LodgeSpotGo.Notification.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.Notification.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.Notification.Grpc.dll"]
