using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lolguesser.Models;

namespace lolguesser.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private DataDragonModel _apiModel;



    public HomeController(ILogger<HomeController> logger)
    {
        _apiModel = new DataDragonModel();
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {

        string randomAbility = await _apiModel.getChampionSpell();
        List<string> imgs = await _apiModel.getChampionsImg();
        ViewData["championsImg"] = imgs;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
