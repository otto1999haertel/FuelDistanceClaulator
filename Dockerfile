# Verwende ein .NET SDK-Image zum Bauen der Anwendung
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Setze das Arbeitsverzeichnis im Container
WORKDIR /src

# Kopiere die Projektdateien in das Arbeitsverzeichnis
COPY . .

# Wiederherstelle NuGet-Pakete
RUN dotnet restore "FuelDistanceCalculator/FuelDistanceCalculator.csproj"

# Baue die Anwendung
RUN dotnet publish "FuelDistanceCalculator/FuelDistanceCalculator.csproj" -c Release -o /app/publish --no-restore

# Verwende ein Laufzeit-Image für die finale App
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Kopiere die gebaute Anwendung aus dem Build-Container
COPY --from=build /app/publish .

# Definiere den Startbefehl
ENTRYPOINT ["dotnet", "FuelDistanceCalculator.dll"]