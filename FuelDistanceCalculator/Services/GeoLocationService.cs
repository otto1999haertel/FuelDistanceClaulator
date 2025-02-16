using System;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace FuelDistanceCalculator.Services;

public class GeoLocationService
{
    public Task<string> GetCoordinatesAsync(string place)
    {
        return Task.FromResult("Latitude: 52.5200, Longitude: 13.4050");
    }

}
