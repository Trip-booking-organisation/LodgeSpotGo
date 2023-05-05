FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AccommodationManagement.Api/AccommodationManagement.Api.csproj", "AccommodationManagement.Api/"]
RUN dotnet restore "AccommodationManagement.Api/AccommodationManagement.Api.csproj"
COPY . .
WORKDIR "/src/AccommodationManagement.Api"
RUN dotnet build "AccommodationManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccommodationManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccommodationManagement.Api.dll"]
