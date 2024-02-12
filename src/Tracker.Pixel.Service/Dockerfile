FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Docker
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Tracker.Pixel.Service/Tracker.Pixel.Service.csproj", "src/Tracker.Pixel.Service/"]
RUN dotnet restore "src/Tracker.Pixel.Service/Tracker.Pixel.Service.csproj"
COPY . .
WORKDIR "/src/src/Tracker.Pixel.Service"
RUN dotnet build "Tracker.Pixel.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tracker.Pixel.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS unit-tests

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.Pixel.Service.dll", "--environment=Docker"]
