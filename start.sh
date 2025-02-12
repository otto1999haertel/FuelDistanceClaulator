#!/bin/bash

# Setze das Passwort für PostgreSQL (kann aus den Umgebungsvariablen übernommen werden)
export PGPASSWORD=example

# Warten, bis die DB verfügbar ist
until psql -h db -U postgres -d FuelDatabase -c '\q'; do
  echo "Warte auf DB..."
  sleep 2
done

# Führe das SQL-Skript aus, um die Tabellen zu erstellen
psql -h db -U postgres -d FuelDatabase -f /app/create_tables.sql

# Starte die Anwendung
dotnet FuelDistanceCalculator.dll
