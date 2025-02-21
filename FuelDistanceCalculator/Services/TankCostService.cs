public static class TankCostService{
    public static List<GasStation> GetCheapestStations(List<GasStation> stations, double fuelAmount, double costPerKm)
    {
        var stationCosts = new List<(GasStation Station, double TotalCost)>();

        foreach (var station in stations)
        {
            if (station.IsOpen)
            {
                // Spritkosten (Preis pro Liter * Menge des getankten Sprits)
                double fuelCost = station.Price * fuelAmount;

                // Wegkosten (Entfernung in km * Kosten pro Kilometer)
                double travelCost = station.Distance * costPerKm;

                // Gesamtkosten (Spritkosten + Wegkosten)
                double totalCost = fuelCost + travelCost;

                // Füge die Tankstelle und die Gesamtkosten zur Liste hinzu
                stationCosts.Add((station, totalCost));
            }
        }

        // Sortiere die Tankstellen nach den Gesamtkosten (aufsteigend)
        return stationCosts.OrderBy(sc => sc.TotalCost)
                           .Select(sc => sc.Station)
                           .ToList();
    }
}