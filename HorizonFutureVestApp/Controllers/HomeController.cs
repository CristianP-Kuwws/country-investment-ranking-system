using Application.Dtos;
using Application.Interfaces.Services.Calculations;
using Application.Interfaces.Services;
using Application.ViewModels.RankingSimulator.ResultsViewModels;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IMacroIndicatorService _macroIndicatorService;
    private readonly IIndicatorService _indicatorService;
    private readonly IRankingCalculationService _rankingCalculationService;

    public HomeController(
        IMacroIndicatorService macroIndicatorService,
        IIndicatorService indicatorService,
        IRankingCalculationService rankingCalculationService)
    {
        _macroIndicatorService = macroIndicatorService;
        _indicatorService = indicatorService;
        _rankingCalculationService = rankingCalculationService;
    }

    public async Task<IActionResult> Index()
    {
        var years = await _indicatorService.GetDistinctYears();

        ViewBag.AvailableYears = years;
        ViewBag.SelectedYear = years.Any() ? years.Max() : DateTime.Now.Year;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetRanking(int selectedYear)
    {
        var allMacros = await _macroIndicatorService.GetAll();

        var macrosConfig = allMacros.Select(m => new MacroWithWeightDto
        {
            IdMacroIndicator = m.IdMacroIndicator,
            Name = m.Name,
            Weight = m.Weight,
            IsHighBetter = m.IsHighBetter
        }).ToList();

        var result = await _rankingCalculationService.CalculateRanking(selectedYear, macrosConfig);

        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage;
            return RedirectToAction("Index");
        }

        // Un solo pais
        if (result.Results.Count <= 1)
        {
            var vmSingle = new RankingResultsViewModel
            {
                Year = selectedYear,
                Rankings = new List<RankingItemViewModel>(),
                SingleCountryName = result.Results.FirstOrDefault()?.CountryName
            };
            return View("Results", vmSingle);
        }

        // Varios paises
        var vm = new RankingResultsViewModel
        {
            Year = selectedYear,
            Rankings = result.Results.Select((r, i) => new RankingItemViewModel
            {
                Position = i + 1,
                CountryName = r.CountryName,
                IsoCode = r.IsoCode,
                Scoring = r.Scoring,
                EstimatedReturnRate = r.EstimatedReturnRate
            }).ToList()
        };

        return View("Results", vm);
    }
}