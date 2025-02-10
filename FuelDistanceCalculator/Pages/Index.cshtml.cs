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

    public string NameGasStation1 {get;set;}
    public string NameGasStation2 {get;set;}

    public bool Calculated { get; set; }
    public decimal TotalCost1 { get; set; }
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

     public void OnPost()
    {
        // Berechnung der Gesamtkosten für Tankstelle 1
        var service1 = new FuelPriceService((int)FuelAmount, PricePerKm);
        TotalCost1 = service1.CalculateEntireCost(FuelPrice1, Distance1);
        TotalCost2 = service1.CalculateEntireCost(FuelPrice2, Distance2);

        Calculated = true;
    }
}
