FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AccommodationManagement/JetSetGo.AccommodationManagement.Grpc/JetSetGo.AccommodationManagement.Grpc.csproj", "AccommodationManagement/JetSetGo.AccommodationManagement.Grpc/"]
COPY ["AccommodationManagement/JetSetGo.AccommodationManagement.Domain/JetSetGo.AccommodationManagement.Domain.csproj", "AccommodationManagement/JetSetGo.AccommodationManagement.Domain/"]
COPY ["AccommodationManagement/JetSetGo.AccommodationManagement.Application/JetSetGo.AccommodationManagement.Application.csproj", "AccommodationManagement/JetSetGo.AccommodationManagement.Application/"]
COPY ["AccommodationManagement/JetSetGo.AccommodationManagement.Infrastructure/JetSetGo.AccommodationManagement.Infrastructure.csproj", "AccommodationManagement/JetSetGo.AccommodationManagement.Infrastructure/"]
COPY ["SharedEvents/LodgeSpotGo.Shared.Events/LodgeSpotGo.Shared.Events.csproj", "SharedEvents/LodgeSpotGo.Shared.Events/"]
RUN dotnet restore "AccommodationManagement/JetSetGo.AccommodationManagement.Grpc/JetSetGo.AccommodationManagement.Grpc.csproj"

COPY AccommodationManagement/JetSetGo.AccommodationManagement.Grpc/ AccommodationManagement/JetSetGo.AccommodationManagement.Grpc/
COPY AccommodationManagement/JetSetGo.AccommodationManagement.Application/ AccommodationManagement/JetSetGo.AccommodationManagement.Application/
COPY AccommodationManagement/JetSetGo.AccommodationManagement.Domain/ AccommodationManagement/JetSetGo.AccommodationManagement.Domain/
COPY AccommodationManagement/JetSetGo.AccommodationManagement.Infrastructure/ AccommodationManagement/JetSetGo.AccommodationManagement.Infrastructure/
COPY SharedEvents/LodgeSpotGo.Shared.Events/ SharedEvents/LodgeSpotGo.Shared.Events/

WORKDIR "/src/AccommodationManagement/JetSetGo.AccommodationManagement.Grpc"
RUN dotnet build "JetSetGo.AccommodationManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.AccommodationManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.AccommodationManagement.Grpc.dll"]

