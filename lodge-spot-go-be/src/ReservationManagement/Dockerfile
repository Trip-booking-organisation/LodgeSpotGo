FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ReservationManagement/JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj", "ReservationManagement/JetSetGo.ReservationManagement.Grpc/"]
COPY ["ReservationManagement/JetSetGo.ReservationManagement.Domain/JetSetGo.ReservationManagement.Domain.csproj", "ReservationManagement/JetSetGo.ReservationManagement.Domain/"]
COPY ["ReservationManagement/JetSetGo.ReservationManagement.Application/JetSetGo.ReservationManagement.Application.csproj", "ReservationManagement/JetSetGo.ReservationManagement.Application/"]
COPY ["ReservationManagement/JetSetGo.ReservationManagement.Infrastructure/JetSetGo.ReservationManagement.Infrastructure.csproj", "ReservationManagement/JetSetGo.ReservationManagement.Infrastructure/"]
COPY ["SharedEvents/LodgeSpotGo.Shared.Events/LodgeSpotGo.Shared.Events.csproj", "SharedEvents/LodgeSpotGo.Shared.Events/LodgeSpotGo.Shared.Events/"]
RUN dotnet restore "ReservationManagement/JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj"

COPY ReservationManagement/JetSetGo.ReservationManagement.Grpc/ ReservationManagement/JetSetGo.ReservationManagement.Grpc/
COPY ReservationManagement/JetSetGo.ReservationManagement.Domain/ ReservationManagement/JetSetGo.ReservationManagement.Domain/
COPY ReservationManagement/JetSetGo.ReservationManagement.Application/ ReservationManagement/JetSetGo.ReservationManagement.Application/
COPY ReservationManagement/JetSetGo.ReservationManagement.Infrastructure/ ReservationManagement/JetSetGo.ReservationManagement.Infrastructure/
COPY SharedEvents/LodgeSpotGo.Shared.Events/ SharedEvents/LodgeSpotGo.Shared.Events/

WORKDIR "/src/ReservationManagement/JetSetGo.ReservationManagement.Grpc"

RUN dotnet build "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.ReservationManagement.Grpc.dll"]
