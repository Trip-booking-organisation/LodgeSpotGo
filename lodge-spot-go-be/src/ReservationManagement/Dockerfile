FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj", "JetSetGo.ReservationManagement.Grpc/"]
COPY ["JetSetGo.ReservationManagement.Domain/JetSetGo.ReservationManagement.Domain.csproj", "JetSetGo.ReservationManagement.Domain/"]
COPY ["JetSetGo.ReservationManagement.Application/JetSetGo.ReservationManagement.Application.csproj", "JetSetGo.ReservationManagement.Application/"]
COPY ["JetSetGo.ReservationManagement.Infrastructure/JetSetGo.ReservationManagement.Infrastructure.csproj", "JetSetGo.ReservationManagement.Infrastructure/"]
RUN dotnet restore "JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj"
COPY . .
WORKDIR "/src/JetSetGo.ReservationManagement.Grpc"
RUN dotnet build "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.ReservationManagement.Grpc.dll"]

