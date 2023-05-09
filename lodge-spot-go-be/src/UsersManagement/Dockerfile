FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/UsersManagement/JetSetGo.UsersManagement.Api/JetSetGo.UsersManagement.Api.csproj", "src/UsersManagement/JetSetGo.UsersManagement.Api/"]
RUN dotnet restore "src/UsersManagement/JetSetGo.UsersManagement.Api/JetSetGo.UsersManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/UsersManagement/JetSetGo.UsersManagement.Api"
RUN dotnet build "JetSetGo.UsersManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.UsersManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.UsersManagement.Api.dll"]
