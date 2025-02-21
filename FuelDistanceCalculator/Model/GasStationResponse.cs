using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class GasStationResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("stations")]
    public List<GasStation> Stations { get; set; }
}