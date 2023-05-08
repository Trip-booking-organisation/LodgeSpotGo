FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/AccommodationManagement/JetSetGo.AccommodationManagement.Api/JetSetGo.AccommodationManagement.Api.csproj", "src/AccommodationManagement/JetSetGo.AccommodationManagement.Api/"]
RUN dotnet restore "src/AccommodationManagement/JetSetGo.AccommodationManagement.Api/JetSetGo.AccommodationManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/AccommodationManagement/JetSetGo.AccommodationManagement.Api"
RUN dotnet build "JetSetGo.AccommodationManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.AccommodationManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.AccommodationManagement.Api.dll"]
