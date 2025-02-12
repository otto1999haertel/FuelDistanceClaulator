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

     public IList<tankinfomodel> Tankinfos { get; set; }

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

    public async Task OnPostAsync(string action)
    {
    if (Enum.TryParse<ActionType>(action, true, out var actionType))
    {
        switch (actionType)
        {
            case ActionType.DeleteHistory:
                Console.WriteLine("History was deleted");

                // Prüfen, ob der Kontext existiert
                if (_context != null)
                {
                    var allEntries = await _context.TankinfoModel.ToListAsync();
                    if (allEntries.Any())
                    {
                        _context.TankinfoModel.RemoveRange(allEntries);
                        await _context.SaveChangesAsync();
                    }
                }
                break;
        }
    }

    // Stelle sicher, dass Tankinfos aktualisiert wird, damit Razor die neue Liste kennt
    Tankinfos = await _context.TankinfoModel.ToListAsync();
    }
}