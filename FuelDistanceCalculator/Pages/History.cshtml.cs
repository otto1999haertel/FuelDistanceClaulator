using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuelDistanceCalculator.Pages;

public class HistoryModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public HistoryModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ViewData["ContactName"] = ContactInfo.Name;
    }

    public void OnPost(string action){
        if (Enum.TryParse<ActionType>(action, true, out var actionType))
        {
            switch (actionType){
                case ActionType.DeleteHistory:
                Console.WriteLine("History was deleted");
            break;
        }
        }
    }
}