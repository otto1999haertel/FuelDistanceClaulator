using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class HistoryModel : PageModel
{
    private readonly ILogger<HistoryModel> _logger;

    // Die Liste von gespeicherten Berechnungen
    public List<HistoryItem> HistoryItems { get; set; }

     public bool IsCsvExportImportEnabled { get; set; }

    public HistoryModel(ILogger<HistoryModel> logger)
    {
        _logger = logger;

        // Beispiel-Daten
        HistoryItems = new List<HistoryItem>
        {
            new HistoryItem
            {
                Date = DateTime.Now.AddDays(-1),
                FuelAmount=55,
                GasStation1 = "Tankstelle 1",
                FuelPrice1 = 10.50,
                GasStation2 = "Tankstelle 2",
                FuelPrice2 = 11.00,
                FuelType ="Diesel"
            },
            new HistoryItem
            {
                Date = DateTime.Now.AddDays(-2),
                FuelAmount=35,
                GasStation1 = "Tankstelle 3",
                FuelPrice1 = 9.75,
                GasStation2 = "Tankstelle 4",
                FuelPrice2 = 10.20,
                FuelType ="Benzin"
            }
        };
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;
        IsCsvExportImportEnabled = false;
    }

    public void OnPost(string action)
    {
        if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
            switch (actionType)
            {
                case ActionType.DeleteHistory:
                    HistoryItems.Clear();
                    Console.WriteLine("History was deleted");
                break;
            }
        }
    }
}

public class HistoryItem
{
    public DateTime Date { get; set; }
    public string GasStation1 { get; set; }
    public double FuelPrice1 { get; set; }
    public string GasStation2 { get; set; }
    public double FuelPrice2 { get; set; }

    public string FuelType { get; set; }
    public double  FuelAmount{get; set; }
}