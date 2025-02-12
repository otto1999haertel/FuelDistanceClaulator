# Verwende das .NET SDK-Image zum Bauen der Anwendung
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Kopiere den gesamten Code ins Image
COPY . .

# Wiederherstelle NuGet-Pakete
RUN dotnet restore "FuelDistanceCalculator/FuelDistanceCalculator.csproj"

# Baue die Anwendung
RUN dotnet publish "FuelDistanceCalculator/FuelDistanceCalculator.csproj" -c Release -o /app/publish --no-restore

# Installiere den PostgreSQL-Client (für pg_isready)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration

WORKDIR /app

# Kopiere die Anwendung aus dem Build-Container
COPY --from=build /app/publish .

# Installiere PostgreSQL-Client
RUN apt-get update && apt-get install -y postgresql-client

# Installiere die EF-Tools, falls sie noch nicht verfügbar sind
RUN dotnet tool install --global dotnet-ef

# Führe Migrationen aus
RUN dotnet ef database update

# Verwende das .NET Runtime-Image für die finale App
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080

# Kopiere die gebaute Anwendung aus dem Build-Container
COPY --from=build /app/publish .

# Setze den EntryPoint der Anwendung
ENTRYPOINT ["dotnet", "FuelDistanceCalculator.dll"]
