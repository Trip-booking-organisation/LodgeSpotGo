FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SearchAndBooking.Api/SearchAndBooking.Api.csproj", "SearchAndBooking.Api/"]
RUN dotnet restore "SearchAndBooking.Api/SearchAndBooking.Api.csproj"
COPY . .
WORKDIR "/src/SearchAndBooking.Api"
RUN dotnet build "SearchAndBooking.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchAndBooking.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchAndBooking.Api.dll"]
