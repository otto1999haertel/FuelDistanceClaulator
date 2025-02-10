using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly FuelPriceService _fuelPriceService;

    [BindProperty]
    public decimal FuelAmount { get; set; } // Globale Tankmenge für beide Tankstellen
    [BindProperty]
    public decimal PricePerKm { get; set; } // Preis pro Kilometer für beide Tankstellen

    [BindProperty]
    public decimal Distance1 { get; set; }
    [BindProperty]
    public decimal FuelPrice1 { get; set; }

    [BindProperty]
    public decimal Distance2 { get; set; }
    [BindProperty]
    public decimal FuelPrice2 { get; set; }
    
    [BindProperty]
    public string NameGasStation1 {get;set;}

    [BindProperty]
    public string NameGasStation2 {get;set;}
       
    [BindProperty]
    public bool CalculationSucessful { get; set; }

    [BindProperty]
    public decimal TotalCost1 { get; set; }

    [BindProperty]
    public decimal TotalCost2 { get; set; }

    public IndexModel(ILogger<IndexModel> logger, FuelPriceService fuelPrice)
    {
        _logger = logger;
        _fuelPriceService = fuelPrice;
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;

        FuelAmount = 0;
        PricePerKm = 0.25m;
        FuelPrice1 = 0;
        Distance1 = 0;
        FuelPrice2 = 0;
        Distance2 = 0;
    }


     public void OnPost(string action)
    {
       if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
        switch (actionType)
        {
            case ActionType.Calculate:
                // Berechnung durchführen
                var service1 = new FuelPriceService((int)FuelAmount, PricePerKm);
                TotalCost1 = service1.CalculateEntireCost(FuelPrice1, Distance1);
                TotalCost2 = service1.CalculateEntireCost(FuelPrice2, Distance2);
                if (TotalCost1 > 0 && TotalCost2 > 0)
                {
                    CalculationSucessful = true;
                }
                break;

            case ActionType.Save:
                // Speichern durchführen
                DateTime saveDate = DateTime.Now;
                Console.WriteLine(saveDate.ToString("dddd, dd MMMM yyyy HH:mm"));
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