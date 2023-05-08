FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SearchAndFilter/JetSetGo.SearchAndFilter.Api/JetSetGo.SearchAndFilter.Api.csproj", "src/SearchAndFilter/JetSetGo.SearchAndFilter.Api/"]
RUN dotnet restore "src/SearchAndFilter/JetSetGo.SearchAndFilter.Api/JetSetGo.SearchAndFilter.Api.csproj"
COPY . .
WORKDIR "/src/src/SearchAndFilter/JetSetGo.SearchAndFilter.Api"
RUN dotnet build "JetSetGo.SearchAndFilter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.SearchAndFilter.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.SearchAndFilter.Api.dll"]
