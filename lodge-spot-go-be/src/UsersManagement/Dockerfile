FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["JetSetGo.UsersManagement.Grpc/JetSetGo.UsersManagement.Grpc.csproj", "JetSetGo.UsersManagement.Grpc/"]
COPY ["JetSetGo.UsersManagement.Domain/JetSetGo.UsersManagement.Domain.csproj", "JetSetGo.UsersManagement.Domain/"]
COPY ["JetSetGo.UsersManagement.Application/JetSetGo.UsersManagement.Application.csproj", "JetSetGo.UsersManagement.Application/"]
COPY ["JetSetGo.UsersManagement.Infrastructure/JetSetGo.UsersManagement.Infrastructure.csproj", "JetSetGo.UsersManagement.Infrastructure/"]
RUN dotnet restore "JetSetGo.UsersManagement.Grpc/JetSetGo.UsersManagement.Grpc.csproj"
COPY . .
WORKDIR "/src/JetSetGo.UsersManagement.Grpc"
RUN dotnet build "JetSetGo.UsersManagement.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetSetGo.UsersManagement.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetSetGo.UsersManagement.Grpc.dll"]

