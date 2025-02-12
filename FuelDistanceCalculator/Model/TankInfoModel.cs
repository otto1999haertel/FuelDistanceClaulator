namespace FuelDistanceCalculator.Model{
public class TankinfoModel
{
    public int Id { get; set; }  // Primärschlüssel (ID der Zeile)

    public DateTime Date { get; set; } // Datum der Erfassung

    public string FuelType { get; set; } // Spritart (z.B. Benzin, Diesel)

    public double FuelAmount { get; set; } // Menge des getankten Sprits (in Litern)

    public string NameGasStation1 { get; set; } // Name oder Adresse von Tankstelle 1

    public double FuelPrice1 { get; set; } // Preis des Sprits an Tankstelle 1

    public string NameGasStation2 { get; set; } // Name oder Adresse von Tankstelle 2

    public double FuelPrice2 { get; set; } // Preis des Sprits an Tankstelle 2
}
}
