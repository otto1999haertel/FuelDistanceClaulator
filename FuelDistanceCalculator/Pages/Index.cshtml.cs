using FuelDistanceCalculator.Constants;
using FuelDistanceCalculator.Data;
using FuelDistanceCalculator.Model;
using FuelDistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

      private readonly AppDbContext _context;
    private FuelCostService _fuelPriceService;

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

    [BindProperty]
    public double FuelAmountBreakEven { get; set; }

    [BindProperty]
    public string NameGasStationBreakEven{get;set;}

    [BindProperty]
    public bool BreakEvenAnalysisDeterministic {get;set;}
    
    
    [BindProperty] 
    public InputMode SelectInputMode {get;set;}  = InputMode.man;

    [BindProperty]
    public VolumeUnit VolumeUnit{
            get => SelectedFuelType == FuelType.Elektro ? VolumeUnit.kwh : VolumeUnit.Liter;
    }

    [BindProperty]
    public int Radius {get; set;}   

    [BindProperty]
    public string Place {get; set;} 

    public IndexModel(ILogger<IndexModel> logger, FuelCostService fuelPrice, AppDbContext context) 
    {
        _logger = logger;
        _fuelPriceService = fuelPrice;
          _context = context;
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;
        NameGasStation1 = "Tankstelle 1";
        NameGasStation2 = "Tankstelle 2";
        SelectedFuelType = FuelType.Diesel;
        SelectInputMode = InputMode.man;


        Console.WriteLine("get was executed and overwirte of values");
        FuelAmount = 0;
        PricePerKm = 0.25;
        FuelPrice1 = 0;
        Distance1 = 0;
        FuelPrice2 = 0;
        Distance2 = 0;
        Radius = 1;
        Place = "Your place";

        if (TempData["TotalCost1"] != null && TempData["TotalCost2"] != null)
        {
            TotalCost1 = Convert.ToDouble(TempData["TotalCost1"]);
            TotalCost2 = Convert.ToDouble(TempData["TotalCost2"]);
            CalculationSucessful = true; // Falls es berechnete Werte gibt, setze auf erfolgreich
        }
    }

   public async Task OnPostAsync(ActionType action)
{
    Console.WriteLine($"Total cost {TotalCost1}");
    Console.WriteLine("Action " + action);
    _fuelPriceService = new FuelCostService((int)FuelAmount, PricePerKm);

    switch (action)
    {
        case ActionType.Calculate:
            Console.WriteLine("calculate");

            TotalCost1 = _fuelPriceService.CalculateEntireCost(FuelPrice1, Distance1);
            TotalCost2 = _fuelPriceService.CalculateEntireCost(FuelPrice2, Distance2);

            if (TotalCost1 > 0 && TotalCost2 > 0)
            {
                Console.WriteLine($"{NameGasStation1} : {TotalCost1}");
                Console.WriteLine($"{NameGasStation2} : {TotalCost2}");
                CalculationSucessful = true;
                TempData["FuelPrice1"] = TotalCost1.ToString();
                TempData["FuelPrice2"] = TotalCost2.ToString();

                string[] tempBreakEvenAnalysis = await Task.Run(() => 
                    _fuelPriceService.AnalyseBreakEven(FuelPrice1, Distance1, NameGasStation1, FuelPrice2, Distance2, NameGasStation2));

                if (tempBreakEvenAnalysis.Length == 2)
                {
                    NameGasStationBreakEven = tempBreakEvenAnalysis[0];
                    if (double.TryParse(tempBreakEvenAnalysis[1], out double ergTemp))
                    {
                        FuelAmountBreakEven = ergTemp;
                    }
                    BreakEvenAnalysisDeterministic = true;
                }
                else
                {
                    BreakEvenAnalysisDeterministic = false;
                }
            }
            break;

        case ActionType.Save:
            Console.WriteLine("save");

            DateTime germanTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin"));
            DateTime dbTime = DateTime.SpecifyKind(germanTime, DateTimeKind.Local);

            var tankinfo = new tankinfomodel
            {
                timesaved = dbTime.ToString("dd.MM.yyyy HH:mm"),
                fueltype = SelectedFuelType.ToString(),
                fuelamount = FuelAmount,
                namegasstation1 = NameGasStation1,
                fuelprice1 = FuelPrice1,
                namegasstation2 = NameGasStation2,
                fuelprice2 = FuelPrice2
            };

            _context.TankinfoModel.Add(tankinfo);
            await _context.SaveChangesAsync(); // Asynchrone Speicherung

            TempData["Message"] = "Daten wurden erfolgreich gespeichert!";
            break;

        case ActionType.Search:
            Console.WriteLine("Search will be executed");
            Console.WriteLine("Input mode in search case: " + SelectInputMode.ToString());
            Console.WriteLine("Radius " + Radius);
            Console.WriteLine("Place " + Place);
            Console.WriteLine("Fuel type  " + SelectedFuelType.ToString());
            Console.WriteLine("Fuel Amount " + FuelAmount);

            // API-Aufruf zur Koordinatensuche
            var coordinates = await new GeoLocationService().GetCoordinatesAsync(Place);
            if (coordinates != null)
            {
                Console.WriteLine($"Found coordinates: " + coordinates);
                // Weitere Verarbeitung...
            }
            break;

        default:
            TempData["Message"] = "Unbekannte Aktion.";
            break;
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