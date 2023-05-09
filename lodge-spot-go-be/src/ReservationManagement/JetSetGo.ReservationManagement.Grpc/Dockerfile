FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ReservationManagement/JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj", "src/ReservationManagement/JetSetGo.ReservationManagement.Grpc/"]
RUN dotnet restore "src/ReservationManagement/JetSetGo.ReservationManagement.Grpc/JetSetGo.ReservationManagement.Grpc.csproj"
COPY . .
WORKDIR "/src/src/ReservationManagement/JetSetGo.ReservationManagement.Grpc"
RUN dotnet build "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.ReservationManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.ReservationManagement.Grpc.dll"]
