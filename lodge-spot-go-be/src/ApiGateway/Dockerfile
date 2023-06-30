FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LodgeSpotGo.ApiGateway/LodgeSpotGo.ApiGateway.csproj", "LodgeSpotGo.ApiGateway/"]
RUN dotnet restore "LodgeSpotGo.ApiGateway/LodgeSpotGo.ApiGateway.csproj"

COPY . .

WORKDIR "/src/LodgeSpotGo.ApiGateway"
RUN dotnet build "LodgeSpotGo.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.ApiGateway.dll"]
