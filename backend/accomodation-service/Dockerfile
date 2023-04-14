FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LodgeSpotGo.Api/LodgeSpotGo.Api.csproj", "LodgeSpotGo.Api/"]
RUN dotnet restore "LodgeSpotGo.Api/LodgeSpotGo.Api.csproj"
COPY . .
WORKDIR "/src/LodgeSpotGo.Api"
RUN dotnet build "LodgeSpotGo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.Api.dll"]
