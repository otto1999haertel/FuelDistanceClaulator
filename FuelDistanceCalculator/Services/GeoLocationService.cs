using System;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // Stelle sicher, dass du das NuGet-Paket Newtonsoft.Json installierst


namespace FuelDistanceCalculator.Services;

public class GeoLocationService
{
    private readonly HttpClient _httpClient;

    public GeoLocationService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<CoordinatesDTO> GetCoordinatesAsync(string place)
    {
        var url = $"https://nominatim.openstreetmap.org/search?q={place}&format=json";
        Console.WriteLine("API Request: " + url);

        // Anfrage an die Nominatim API senden
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "YourAppName/1.0 (your@email.com)");
        var response = await _httpClient.SendAsync(request);

        // Sicherstellen, dass die Antwort erfolgreich war
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error fetching coordinates");
        }

        // Antwort in ein JSON-Objekt parsen
        var responseContent = await response.Content.ReadAsStringAsync();
        var json = JArray.Parse(responseContent);

        // Wenn Koordinaten vorhanden sind, extrahiere sie
        if (json.Count > 0)
        {
            var latitude = double.Parse(json[0]["lat"].ToString());
            var longitude = double.Parse(json[0]["lon"].ToString());

            // Koordinaten als DTO zurückgeben
            return new CoordinatesDTO
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }

        // Falls keine Ergebnisse gefunden wurden
        return null;
    }
}
