FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ApiGateway/JetSetGo.ApiGateway/JetSetGo.ApiGateway.csproj", "src/ApiGateway/JetSetGo.ApiGateway/"]
RUN dotnet restore "src/ApiGateway/JetSetGo.ApiGateway/JetSetGo.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway/JetSetGo.ApiGateway"
RUN dotnet build "JetSetGo.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.ApiGateway.dll"]
