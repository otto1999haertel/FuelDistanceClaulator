using System.Text;
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
        IsCsvExportImportEnabled = true;
    }

    public async Task OnGetAsync()
    {
        // Alle Tankinfo-Daten abfragen
        Console.WriteLine("on Get Async ausgeführt");
        Tankinfos = await _context.TankinfoModel.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync(string action)
    {
        Console.WriteLine("On Post asynch entered with " + action);
        if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
            Console.WriteLine("Switch entered with " + actionType);
            switch (actionType)
            {
                case ActionType.DeleteHistory:
                    Console.WriteLine("History was deleted");

                    // Prüfen, ob der Kontext existiert
                    if (_context != null)
                    {
                        var allEntries = await _context.TankinfoModel.ToListAsync();
                        if (allEntries != null && allEntries.Any())
                        {
                            _context.TankinfoModel.RemoveRange(allEntries);
                            await _context.SaveChangesAsync();
                            Console.WriteLine("History deleted successful");
                        }
                        else{
                            Console.WriteLine("no entries found");
                        }
                    }

                    // Stelle sicher, dass Tankinfos nach dem Löschen aktualisiert wird
                    Tankinfos = await _context.TankinfoModel.ToListAsync();

                    // Nach dem Löschen zurück zur Seite
                    return Page();

                case ActionType.ExportCSV:
                    Console.WriteLine("Export to CSV");

                    var tankInfos = await _context.TankinfoModel.ToListAsync(); // Daten abrufen
                    var csv = new StringBuilder();
                    csv.AppendLine("Datum;Spritart;Tankmenge;Tankstelle 1;Spritpreis TS 1;Tankstelle 2;Spritpreis TS 2");

                    foreach (var entry in tankInfos)
                    {
                        csv.AppendLine($"{entry.timesaved};{entry.fueltype};{entry.fuelamount};{entry.namegasstation1};{entry.fuelprice1};{entry.namegasstation2};{entry.fuelprice2}");
                    }

                    var bytes = Encoding.UTF8.GetBytes(csv.ToString());

                    // ✅ Dateidownload zurückgeben
                    return File(bytes, "text/csv", "Tankstellen_Daten.csv");
            }
            Tankinfos = await _context.TankinfoModel.ToListAsync();
            return Page();
        }
        // Stelle sicher, dass Tankinfos aktualisiert wird, damit Razor die neue Liste kennt
        Tankinfos = await _context.TankinfoModel.ToListAsync();
        return Page();
    }
}