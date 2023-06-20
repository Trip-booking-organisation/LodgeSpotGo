FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UsersManagement/JetSetGo.UsersManagement.Grpc/JetSetGo.UsersManagement.Grpc.csproj", "UsersManagement/JetSetGo.UsersManagement.Grpc/"]
COPY ["UsersManagement/JetSetGo.UsersManagement.Domain/JetSetGo.UsersManagement.Domain.csproj", "UsersManagement/UsersManagementJetSetGo.UsersManagement.Domain/"]
COPY ["UsersManagement/JetSetGo.UsersManagement.Application/JetSetGo.UsersManagement.Application.csproj", "UsersManagement/JetSetGo.UsersManagement.Application/"]
COPY ["UsersManagement/JetSetGo.UsersManagement.Infrastructure/JetSetGo.UsersManagement.Infrastructure.csproj", "UsersManagement/JetSetGo.UsersManagement.Infrastructure/"]
COPY ["SharedEvents/LodgeSpotGo.Shared.Events/LodgeSpotGo.Shared.Events.csproj", "SharedEvents/LodgeSpotGo.Shared.Events/LodgeSpotGo.Shared.Events/"]
RUN dotnet restore "UsersManagement/JetSetGo.UsersManagement.Grpc/JetSetGo.UsersManagement.Grpc.csproj"

COPY UsersManagement/JetSetGo.UsersManagement.Grpc/ UsersManagement/JetSetGo.UsersManagement.Grpc/
COPY UsersManagement/JetSetGo.UsersManagement.Domain/ UsersManagement/JetSetGo.UsersManagement.Domain/
COPY UsersManagement/JetSetGo.UsersManagement.Application/ UsersManagement/JetSetGo.UsersManagement.Application/
COPY UsersManagement/JetSetGo.UsersManagement.Infrastructure/ UsersManagement/JetSetGo.UsersManagement.Infrastructure/
COPY SharedEvents/LodgeSpotGo.Shared.Events/ SharedEvents/LodgeSpotGo.Shared.Events/

WORKDIR "/src/UsersManagement/JetSetGo.UsersManagement.Grpc"
RUN dotnet build "JetSetGo.UsersManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.UsersManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.UsersManagement.Grpc.dll"]

