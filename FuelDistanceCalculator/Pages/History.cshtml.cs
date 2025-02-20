using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
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

    [BindProperty]
    public IFormFile UploadedFile { get; set; }

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
        Console.WriteLine(Tankinfos.ToString());
    }

    public async Task<IActionResult> OnPostDeleteHistory()
{
    Console.WriteLine("OnPostDeleteHistory wurde aufgerufen");

    if (_context == null)
    {
        Console.WriteLine("Fehler: _context ist null!");
        return Page();
    }

    Console.WriteLine("Context: " + _context);
    Console.WriteLine("TankinfoModel: " + (_context.TankinfoModel != null ? "Exists" : "Null"));

    var allEntries = await _context.TankinfoModel.ToListAsync();
    if (allEntries.Any())
    {
        _context.TankinfoModel.RemoveRange(allEntries);
        await _context.SaveChangesAsync();
    }

    Tankinfos = await _context.TankinfoModel.ToListAsync();
    return Page();  
    }

    public async Task<IActionResult> OnPostExportCSV(){
        Console.WriteLine("Export to CSV");

        var tankInfos = await _context.TankinfoModel.ToListAsync(); // Daten abrufen
        var csv = new StringBuilder();
        csv.AppendLine("id;timesaved;fueltype;fuelamount;namegasstation1;fuelprice1;namegasstation2;fuelprice2");
        Console.WriteLine(csv.ToString());
                    foreach (var entry in tankInfos)
                    {
                        csv.AppendLine($"{entry.id};{entry.timesaved};{entry.fueltype};{entry.fuelamount};{entry.namegasstation1};{entry.fuelprice1};{entry.namegasstation2};{entry.fuelprice2}");
                    }

                    var bytes = Encoding.UTF8.GetBytes(csv.ToString());

                    // ✅ Dateidownload zurückgeben
                    return File(bytes, "text/csv", "Tankstellen_Daten.csv");
    }
    public async Task<IActionResult> OnPostImportCSVAsync()
    {
        if (UploadedFile != null && UploadedFile.Length > 0)
        {
            Console.WriteLine("csv import was called");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using (var stream = new StreamReader(UploadedFile.OpenReadStream()))
            //TODO Compare Hash / id
            using (var csv = new CsvReader(stream, config))
            {
                Console.WriteLine(csv.ToString());
                var records = csv.GetRecords<tankinfomodel>().ToList();
                var tankInfos = await _context.TankinfoModel.ToListAsync(); // Daten abrufen
                var existingIds = new HashSet<int>(tankInfos.Select(t => t.id));
                foreach (var record in records)
                {
                    if (!existingIds.Contains(record.id))
                    {
                        Console.WriteLine("range was added");
                        _context.TankinfoModel.Add(record);
                        existingIds.Add(record.id); // Um auch im nächsten Durchgang den neuen Datensatz zu erkennen
                    }
                    else
                    {
                        Console.WriteLine("range was not added");
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
        return RedirectToPage();
    }
}