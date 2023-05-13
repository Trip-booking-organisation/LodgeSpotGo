FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/LodgeSpotGo.SearchAndFilter.Grpc.csproj", "src/SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/"]
RUN dotnet restore "src/SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/LodgeSpotGo.SearchAndFilter.Grpc.csproj"
COPY . .
WORKDIR "/src/src/SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc"
RUN dotnet build "LodgeSpotGo.SearchAndFilter.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.SearchAndFilter.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.SearchAndFilter.Grpc.dll"]
