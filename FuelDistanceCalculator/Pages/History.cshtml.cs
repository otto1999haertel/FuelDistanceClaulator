using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class HistoryModel : PageModel
{
    private readonly ILogger<HistoryModel> _logger;

    // Die Liste von gespeicherten Berechnungen
    public List<HistoryItem> HistoryItems { get; set; }

    public HistoryModel(ILogger<HistoryModel> logger)
    {
        _logger = logger;

        // Beispiel-Daten
        HistoryItems = new List<HistoryItem>
        {
            new HistoryItem
            {
                Date = DateTime.Now.AddDays(-1),
                GasStation1 = "Tankstelle 1",
                TotalCost1 = 10.50,
                GasStation2 = "Tankstelle 2",
                TotalCost2 = 11.00
            },
            new HistoryItem
            {
                Date = DateTime.Now.AddDays(-2),
                GasStation1 = "Tankstelle 3",
                TotalCost1 = 9.75,
                GasStation2 = "Tankstelle 4",
                TotalCost2 = 10.20
            }
        };
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;
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
    public double TotalCost1 { get; set; }
    public string GasStation2 { get; set; }
    public double TotalCost2 { get; set; }
}