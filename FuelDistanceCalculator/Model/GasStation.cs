using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class GasStation
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; }

    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lng")]
    public double Longitude { get; set; }

    [JsonPropertyName("dist")]
    public double Distance { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }  // Hier wird der Preis gespeichert

    [JsonPropertyName("isOpen")]
    public bool IsOpen { get; set; }

    [JsonPropertyName("houseNumber")]
    public string HouseNumber { get; set; }

    [JsonPropertyName("postCode")]
    public int PostCode { get; set; }

    public decimal CalculateTotalCost(double fuelAmount, double pricePerKm)
    {
            return (decimal)(Price * fuelAmount + Distance * pricePerKm);
    }


    // Überschreiben der ToString-Methode für bessere Debug-Ausgabe
    public override string ToString()
    {
        return $"GasStation Info:\n" +
           $"- Id: {Id}\n" +
           $"- Name: {Name}\n" +
           $"- Brand: {Brand}\n" +
           $"- Street: {Street}\n" +
           $"- Place: {Place}\n" +
           $"- Coordinates: {Latitude}, {Longitude}\n" +
           $"- Distance: {Distance} km\n" +
           $"- Price: {Price} EUR\n" +
           $"- Is Open: {(IsOpen ? "Yes" : "No")}\n" +
           $"- House Number: {HouseNumber}\n" +
           $"- PostCode: {PostCode}";
    }
}
