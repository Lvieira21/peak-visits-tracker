FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
ENV DOTNET_ENVIRONMENT=Docker
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Tracker.Storage.Service/Tracker.Storage.Service.csproj", "src/Tracker.Storage.Service/"]
RUN dotnet restore "src/Tracker.Storage.Service/Tracker.Storage.Service.csproj"
COPY . .
WORKDIR "/src/src/Tracker.Storage.Service"
RUN dotnet build "Tracker.Storage.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tracker.Storage.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Storage.Service.dll", "--environment=Docker"]
