FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["JetSetGo.AccommodationManagement.Grpc/JetSetGo.AccommodationManagement.Grpc.csproj", "JetSetGo.AccommodationManagement.Grpc/"]
COPY ["JetSetGo.AccommodationManagement.Domain/JetSetGo.AccommodationManagement.Domain.csproj", "JetSetGo.AccommodationManagement.Domain/"]
COPY ["JetSetGo.AccommodationManagement.Application/JetSetGo.AccommodationManagement.Application.csproj", "JetSetGo.AccommodationManagement.Application/"]
COPY ["JetSetGo.AccommodationManagement.Infrastructure/JetSetGo.AccommodationManagement.Infrastructure.csproj", "JetSetGo.AccommodationManagement.Infrastructure/"]
RUN dotnet restore "JetSetGo.AccommodationManagement.Grpc/JetSetGo.AccommodationManagement.Grpc.csproj"
COPY . .
WORKDIR "/src/JetSetGo.AccommodationManagement.Grpc"
RUN dotnet build "JetSetGo.AccommodationManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.AccommodationManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.AccommodationManagement.Grpc.dll"]

