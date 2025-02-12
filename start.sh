#!/bin/bash

# Warte, bis PostgreSQL bereit ist
echo "Warte auf PostgreSQL..."
until pg_isready -h db -p 5432; do
  sleep 2
done

# Führe die Migrationen aus
echo "Führe Migrationen aus..."
dotnet ef database update --no-build

# Starte die Anwendung
dotnet FuelDistanceCalculator.dll
