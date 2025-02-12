using FuelDistanceCalculator.Data;
using FuelDistanceCalculator.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FuelDistanceCalculator.Pages;

public class HistoryModel : PageModel
{
    private readonly ILogger<HistoryModel> _logger;

    // Die Liste von gespeicherten Berechnungen

     public bool IsCsvExportImportEnabled { get; set; }

     private readonly AppDbContext _context;

     public IList<TankinfoModel> Tankinfos { get; set; }

    public HistoryModel(ILogger<HistoryModel> logger, AppDbContext context)
    {
        _logger = logger;

        _context = context;
    }

     public async Task OnGetAsync()
    {
        // Alle Tankinfo-Daten abfragen
        Console.WriteLine("on Get Async ausgeführt");
        Tankinfos = await _context.TankinfoModel.ToListAsync();
    }

    public void OnPost(string action)
    {
        if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
            switch (actionType)
            {
                case ActionType.DeleteHistory:
                    Console.WriteLine("History was deleted");
                break;
            }
        }
    }
}