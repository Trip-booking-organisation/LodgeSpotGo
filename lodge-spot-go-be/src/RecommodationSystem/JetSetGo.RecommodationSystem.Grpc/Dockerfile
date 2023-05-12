FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/RecommodationSystem/JetSetGo.RecommodationSystem.Grpc/JetSetGo.RecommodationSystem.Grpc.csproj", "src/RecommodationSystem/JetSetGo.RecommodationSystem.Grpc/"]
RUN dotnet restore "src/RecommodationSystem/JetSetGo.RecommodationSystem.Grpc/JetSetGo.RecommodationSystem.Grpc.csproj"
COPY . .
WORKDIR "/src/src/RecommodationSystem/JetSetGo.RecommodationSystem.Grpc"
RUN dotnet build "JetSetGo.RecommodationSystem.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.RecommodationSystem.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.RecommodationSystem.Grpc.dll"]
