using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private FuelPriceService _fuelPriceService;

    [BindProperty]
    public double FuelAmount { get; set; } // Globale Tankmenge für beide Tankstellen
    [BindProperty]
    public double PricePerKm { get; set; } // Preis pro Kilometer für beide Tankstellen

    [BindProperty]
    public double Distance1 { get; set; }
    [BindProperty]
    public double FuelPrice1 { get; set; }

    [BindProperty]
    public double Distance2 { get; set; }
    [BindProperty]
    public double FuelPrice2 { get; set; }
    
    [BindProperty]
    public string NameGasStation1 {get;set;}

    [BindProperty]
    public string NameGasStation2 {get;set;}
       
    [BindProperty]
    public bool CalculationSucessful { get; set; }

    [BindProperty]
    public double TotalCost1 { get; private set; }

    [BindProperty]
    public double TotalCost2 { get; private set; }

    [BindProperty]
    public FuelType SelectedFuelType { get; set; }



    public IndexModel(ILogger<IndexModel> logger, FuelPriceService fuelPrice)
    {
        _logger = logger;
        _fuelPriceService = fuelPrice;
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;
        NameGasStation1 = "Tankstelle 1";
        NameGasStation2 = "Tankstelle 2";
        SelectedFuelType = FuelType.Diesel;


        Console.WriteLine("get was executed and overwirte of values");
        FuelAmount = 0;
        PricePerKm = 0.25;
        FuelPrice1 = 0;
        Distance1 = 0;
        FuelPrice2 = 0;
        Distance2 = 0;

        if (TempData["TotalCost1"] != null && TempData["TotalCost2"] != null)
        {
            TotalCost1 = Convert.ToDouble(TempData["TotalCost1"]);
            TotalCost2 = Convert.ToDouble(TempData["TotalCost2"]);
            CalculationSucessful = true; // Falls es berechnete Werte gibt, setze auf erfolgreich
        }
    }


     public void OnPost(string action)
    {
        Console.WriteLine($"Total cost{TotalCost1}");
        _fuelPriceService = new FuelPriceService((int)FuelAmount, PricePerKm);
       if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
        switch (actionType)
        {
            case ActionType.Calculate:
                // Berechnung durchführen
                Console.WriteLine("calculate");

                TotalCost1 = _fuelPriceService.CalculateEntireCost(FuelPrice1, Distance1);
                TotalCost2 = _fuelPriceService.CalculateEntireCost(FuelPrice2, Distance2);
                if (TotalCost1 > 0 && TotalCost2 > 0)
                {
                    Console.WriteLine($"{NameGasStation1} : {TotalCost1}");
                     Console.WriteLine($"{NameGasStation2} : {TotalCost2}");
                    CalculationSucessful = true;
                    TempData["FuelPrice1"] = TotalCost1.ToString(); // Speichern in TempData
                    TempData["FuelPrice2"] = TotalCost2.ToString(); // Speichern in TempData
                }
                break;

            case ActionType.Save:
                Console.WriteLine("save");
                // Speichern durchführen
                DateTime saveDate = DateTime.Now;

                Console.WriteLine(saveDate.ToString("dddd, dd MMMM yyyy HH:mm"));
                Console.WriteLine($"{NameGasStation1} : {TempData["FuelPrice1"]}");
                Console.WriteLine($"{NameGasStation2} : {TempData["FuelPrice2"]}");
                Console.WriteLine($"Ausgewählte Spritart: {SelectedFuelType}");
                TempData["Message"] = "Daten wurden NICHT erfolgreich gespeichert!";
                break;

            default:
                // Unbekannte Aktion
                TempData["Message"] = "Unbekannte Aktion.";
                break;
        }
        }
        else
         {
        TempData["Message"] = "Ungültiger Aktionswert.";
         }

        ViewData["ContactName"] = ContactInfo.Name;

    }

    // Speichern-Methode, wird durch den Speichern-Button ausgelöst
    public IActionResult OnPostSaveData()
    {
         _logger.LogInformation("Speichern-Methode wurde aufgerufen.");  // Loggen für Debugging
        // Dummy-Speichern-Logik (diese wird später durch eine DB ersetzt)
        TempData["Message"] = "Daten wurden nicht erfolgreich gespeichert!";

        // Weiterleitung zurück zur Index-Seite
        return RedirectToPage();
    }   
}