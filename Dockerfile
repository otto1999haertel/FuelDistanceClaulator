# Verwende das .NET SDK-Image zum Bauen der Anwendung
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

# Wiederherstelle NuGet-Pakete
RUN dotnet restore "FuelDistanceCalculator/FuelDistanceCalculator.csproj"

# Baue die Anwendung
RUN dotnet publish "FuelDistanceCalculator/FuelDistanceCalculator.csproj" -c Release -o /app/publish --no-restore

# Verwende das .NET Runtime-Image für die finale App
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Kopiere die gebaute Anwendung aus dem Build-Container
COPY --from=build /app/publish .

# Stelle sicher, dass das .NET SDK auch im Container vorhanden ist, falls Migrationen ausgeführt werden müssen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration

WORKDIR /app
COPY --from=build /app/publish .

# Hier könntest du EF Migrationen ausführen, falls nötig
# RUN dotnet ef migrations add InitialCreate

ENTRYPOINT ["dotnet", "FuelDistanceCalculator.dll"]
