FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/LodgeSpotGo.SearchAndFilter.Grpc.csproj", "SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/"]
COPY ["SearchAndFilter/JetSetGo.SearchAndFilter.Domain/JetSetGo.SearchAndFilter.Domain.csproj", "SearchAndFilter/JetSetGo.SearchAndFilter.Domain/"]
COPY ["SearchAndFilter/JetSetGo.SearchAndFilter.Application/JetSetGo.SearchAndFilter.Application.csproj", "SearchAndFilter/JetSetGo.SearchAndFilter.Application/"]
COPY ["SearchAndFilter/JetSetGo.SearchAndFilter.Infrastructure/JetSetGo.SearchAndFilter.Infrastructure.csproj", "SearchAndFilter/JetSetGo.SearchAndFilter.Infrastructure/"]
RUN dotnet restore "SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/LodgeSpotGo.SearchAndFilter.Grpc.csproj"

COPY SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/ SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc/
COPY SearchAndFilter/JetSetGo.SearchAndFilter.Domain/ SearchAndFilter/JetSetGo.SearchAndFilter.Domain/
COPY SearchAndFilter/JetSetGo.SearchAndFilter.Application/ SearchAndFilter/JetSetGo.SearchAndFilter.Application/
COPY SearchAndFilter/JetSetGo.SearchAndFilter.Infrastructure/ SearchAndFilter/JetSetGo.SearchAndFilter.Infrastructure/

WORKDIR "/src/SearchAndFilter/LodgeSpotGo.SearchAndFilter.Grpc"
RUN dotnet build "LodgeSpotGo.SearchAndFilter.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LodgeSpotGo.SearchAndFilter.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LodgeSpotGo.SearchAndFilter.Grpc.dll"]

