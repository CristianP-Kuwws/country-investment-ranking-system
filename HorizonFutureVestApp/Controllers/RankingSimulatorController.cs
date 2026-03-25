using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Application.ViewModels.RankingSimulator;
using Application.ViewModels.RankingSimulator.ResultsViewModels;

namespace HorizonFutureVestApp.Controllers
{
    public class RankingSimulatorController : Controller
    {
        private readonly ISimulationService _simulationService;
        private readonly IMacroIndicatorService _macroIndicatorService;
        private readonly IIndicatorService _indicatorService;

        private const string SESSION_KEY = "SimulationConfig";

        public RankingSimulatorController(
            ISimulationService simulationService,
            IMacroIndicatorService macroIndicatorService,
            IIndicatorService indicatorService)
        {
            _simulationService = simulationService;
            _macroIndicatorService = macroIndicatorService;
            _indicatorService = indicatorService;
        }

        // ===== Helpers (Session) =====

        private List<MacroWithWeightDto> GetConfig()
        {
            var json = HttpContext.Session.GetString(SESSION_KEY);
            return string.IsNullOrEmpty(json)
                ? new List<MacroWithWeightDto>()
                : JsonSerializer.Deserialize<List<MacroWithWeightDto>>(json)
                  ?? new List<MacroWithWeightDto>();
        }

        // ==========
        private void SaveConfig(List<MacroWithWeightDto> config)
        {
            HttpContext.Session.SetString(SESSION_KEY, JsonSerializer.Serialize(config));
        }

        public async Task<IActionResult> Index()
        {
            var config = GetConfig();
            var years = await _indicatorService.GetDistinctYears();

            // Convertir a ViewModels
            var macros = config.Select(c => new SimulationMacroViewModel
            {
                IdMacroIndicator = c.IdMacroIndicator,
                Name = c.Name,
                Weight = c.Weight,
                IsHighBetter = c.IsHighBetter
            }).ToList();

            ViewBag.TotalWeight = config.Sum(c => c.Weight);
            ViewBag.CanAddMore = config.Sum(c => c.Weight) < 1m;
            ViewBag.AvailableYears = years;
            ViewBag.SelectedYear = years.Any() ? years.Max() : DateTime.Now.Year;

            return View(macros);
        }

        public async Task<IActionResult> Add()
        {
            var config = GetConfig();

            if (config.Sum(c => c.Weight) >= 1m)
            {
                TempData["Error"] = "No se pueden agregar mas macroindicadores porque la suma de los pesos es 1.";
                return RedirectToAction("Index");
            }

            var allMacros = await _macroIndicatorService.GetAll();
            var usedIds = config.Select(c => c.IdMacroIndicator).ToHashSet();

            var vm = new SaveSimulationMacroViewModel
            {
                Name = "",
                AvailableMacros = allMacros
                    .Where(m => !usedIds.Contains(m.IdMacroIndicator))
                    .Select(m => new MacroOptionViewModel { IdOption = m.IdMacroIndicator, Name = m.Name })
                    .ToList(),
                RemainingWeight = 1m - config.Sum(c => c.Weight),
                Weight = 0
            };

            if (!vm.AvailableMacros.Any())
            {
                TempData["Warning"] = "No hay mas macroindicadores disponibles.";
                return RedirectToAction("Index");
            }

            return View("Save", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SaveSimulationMacroViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                await ReloadMacros(vm);
                return View("Save", vm);
            }

            var config = GetConfig();
            var canAdd = await _simulationService.AddMacroToSimulation(
                config, vm.SelectedMacroIndicator, vm.Weight);

            if (!canAdd)
            {
                ModelState.AddModelError("Weight",
                    $"El peso excede el limite. Disponible: {1m - config.Sum(c => c.Weight):F4}");
                await ReloadMacros(vm);
                return View("Save", vm);
            }

            var macro = await _macroIndicatorService.GetById(vm.SelectedMacroIndicator);

            config.Add(new MacroWithWeightDto
            {
                IdMacroIndicator = macro.IdMacroIndicator,
                Name = macro.Name,
                Weight = vm.Weight,
                IsHighBetter = macro.IsHighBetter
            });

            SaveConfig(config);
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            ViewBag.EditMode = true;

            var config = GetConfig();
            var macro = config.FirstOrDefault(c => c.IdMacroIndicator == id);

            if (macro == null)
                return RedirectToAction("Index");

            var vm = new SaveSimulationMacroViewModel
            {
                IdMacroIndicator = macro.IdMacroIndicator,
                Name = macro.Name,
                Weight = macro.Weight,
                RemainingWeight = 1m - config.Where(c => c.IdMacroIndicator != id).Sum(c => c.Weight)
            };

            return View("Save", vm); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSimulationMacroViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Save", vm);

            var config = GetConfig();
            var canUpdate = await _simulationService.UpdateMacroInSimulation(
                config, vm.IdMacroIndicator, vm.Weight);

            if (!canUpdate)
            {
                var available = 1m - config.Where(c => c.IdMacroIndicator != vm.IdMacroIndicator)
                    .Sum(c => c.Weight);
                ModelState.AddModelError("Weight", $"Limite: {available:F4}");
                return View("Save", vm);
            }

            config.First(c => c.IdMacroIndicator == vm.IdMacroIndicator).Weight = vm.Weight;
            SaveConfig(config);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var config = GetConfig();
            var macro = config.FirstOrDefault(c => c.IdMacroIndicator == id);

            if (macro == null)
                return RedirectToAction("Index");

            return View(new DeleteSimulationMacroViewModel
            {
                IdMacroIndicator = macro.IdMacroIndicator,
            });
        }

        [HttpPost]
        public IActionResult DeleteMacroConfirmed(int id)
        {
            var config = GetConfig();
            config.RemoveAll(c => c.IdMacroIndicator == id);
            SaveConfig(config);
            return RedirectToAction("Index");
        }

        // Ejecutar

        [HttpPost]
        public async Task<IActionResult> RunSimulation(int selectedYear)
        {
            var config = GetConfig();

            if (!config.Any())
            {
                TempData["Error"] = "Debe agregar al menos un macroindicador.";
                return RedirectToAction("Index");
            }

            var result = await _simulationService.RunSimulation(config, selectedYear);

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Index");
            }

            // Si solo hay un pais en los resultados
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

            // Varios paises en el ranking
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


        // Helper
        private async Task ReloadMacros(SaveSimulationMacroViewModel vm)
        {
            var config = GetConfig();
            var all = await _macroIndicatorService.GetAll();
            var used = config.Select(c => c.IdMacroIndicator).ToHashSet();

            vm.AvailableMacros = all
                .Where(m => !used.Contains(m.IdMacroIndicator))
                .Select(m => new MacroOptionViewModel { IdOption = m.IdMacroIndicator, Name = m.Name })
                .ToList();

            vm.RemainingWeight = 1m - config.Sum(c => c.Weight);
        }
    }
}
