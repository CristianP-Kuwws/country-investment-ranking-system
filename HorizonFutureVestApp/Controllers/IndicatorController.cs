using Application.Dtos.Country;
using Application.Dtos.Indicator;
using Application.Dtos.MacroIndicator;
using Application.Interfaces.Services;
using Application.Services;
using Application.ViewModels.Country;
using Application.ViewModels.Indicator;
using Application.ViewModels.MacroIndicator;
using HorizonFutureVestApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVestApp.Controllers
{
    public class IndicatorController : Controller
    {
        private readonly IIndicatorService _indicatorService;
        private readonly ICountryService _countryService;
        private readonly IMacroIndicatorService _macroIndicatorService;

        public IndicatorController(
            IIndicatorService indicatorService,
            ICountryService countryService,
            IMacroIndicatorService macroIndicatorService)
        {
            _indicatorService = indicatorService;
            _countryService = countryService;
            _macroIndicatorService = macroIndicatorService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _indicatorService.GetAllWithIncluded();

            var listEntitiesVms = dtos.Select(s => new IndicatorViewModel()
            {
                IdIndicator = s.IdIndicator,
                IdCountry = s.IdCountry,
                IdMacroIndicator = s.IdMacroIndicator,
                Value = s.Value,
                Year = s.Year,
                CountryName = s.CountryName,
                MacroIndicatorName = s.MacroIndicatorName
            }).ToList();

            return View(listEntitiesVms);
        }

        public async Task<IActionResult> Create()
        {
            // Convertir dtos a viewmodel
            var countries = (await _countryService.GetAll()).ToViewModel();
            var macroIndicators = (await _macroIndicatorService.GetAll()).ToViewModel();

            // Pasar listas al viewmodel
            var model = new SaveIndicatorViewModel
            {
                IdCountry = 0,
                IdMacroIndicator = 0,
                Value = 0,
                Year = DateTime.Now.Year,
                Countries = countries,
                MacroIndicators = macroIndicators
            };

            ViewBag.EditMode = false;
            return View("Save", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveIndicatorViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                await RefillLists(vm); // <-- recargar listas
                ViewBag.EditMode = false;
                return View("Save", vm);
            }

            // Evitar duplicados con mismo MacroIndicador, año, pais
            var all = await _indicatorService.GetAll();
            bool alreadyExists = all.Any(x =>
                x.IdCountry == vm.IdCountry &&
                x.IdMacroIndicator == vm.IdMacroIndicator &&
                x.Year == vm.Year);

            if (alreadyExists)
            {
                ModelState.AddModelError("Year", "Ya existe un indicador para este MacroIndicador, pais y año.");
                await RefillLists(vm); // recargar listas
                return View("Save", vm);
            }

            IndicatorDto dto = new()
            {
                IdIndicator = 0,
                IdCountry = vm.IdCountry,
                IdMacroIndicator = vm.IdMacroIndicator,
                Value = vm.Value,
                Year = vm.Year
            };

            await _indicatorService.AddAsync(dto);
            return RedirectToRoute(new { controller = "Indicator", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _indicatorService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Indicator", action = "Index" });
            }

            var countries = (await _countryService.GetAll()).ToViewModel();
            var macroIndicators = (await _macroIndicatorService.GetAll()).ToViewModel();


            var model = new SaveIndicatorViewModel
            {
                IdIndicator = dto.IdIndicator,
                IdCountry = dto.IdCountry,
                IdMacroIndicator = dto.IdMacroIndicator,
                Value = dto.Value,
                Year = dto.Year,
                Countries = countries,
                MacroIndicators = macroIndicators,
                CountryName = countries.FirstOrDefault(c => c.IdCountry == dto.IdCountry)?.Name,
                MacroIndicatorName = macroIndicators.FirstOrDefault(m => m.IdMacroIndicator == dto.IdMacroIndicator)?.Name
            };

            ViewBag.EditMode = true;
            return View("Save", model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveIndicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await RefillLists(vm); // recargar listas
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            // Evitar duplicados (excluyendo el que se edita)
            var all = await _indicatorService.GetAll();
            bool alreadyExists = all.Any(x =>
                x.IdIndicator != vm.IdIndicator &&
                x.IdCountry == vm.IdCountry &&
                x.IdMacroIndicator == vm.IdMacroIndicator &&
                x.Year == vm.Year);

            if (alreadyExists)
            {
                ModelState.AddModelError("Year", "Ya existe un indicador para este MacroIndicador, pais y año.");
                await RefillLists(vm); // recargar listas
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            IndicatorDto dto = new()
            {
                IdIndicator = vm.IdIndicator,
                IdCountry = vm.IdCountry,
                IdMacroIndicator = vm.IdMacroIndicator,
                Value = vm.Value,
                Year = vm.Year
            };

            await _indicatorService.UpdateAsync(dto);
            return RedirectToRoute(new { controller = "Indicator", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _indicatorService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Indicator", action = "Index" });
            }

            DeleteIndicatorViewModel vm = new()
            {
                IdIndicator = dto.IdIndicator,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteIndicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _indicatorService.DeleteAsync(vm.IdIndicator);
            return RedirectToRoute(new { controller = "Indicator", action = "Index" });
        }

        private async Task RefillLists(SaveIndicatorViewModel model)
        {
            var countries = await _countryService.GetAll() ?? new List<CountryDto>();
            var macroIndicators = await _macroIndicatorService.GetAll() ?? new List<MacroIndicatorDto>();

            model.Countries = countries.ToViewModel() ?? new List<CountryViewModel>();
            model.MacroIndicators = macroIndicators.ToViewModel() ?? new List<MacroIndicatorViewModel>();

            model.CountryName = model.Countries.FirstOrDefault(c => c.IdCountry == model.IdCountry)?.Name;
            model.MacroIndicatorName = model.MacroIndicators.FirstOrDefault(m => m.IdMacroIndicator == model.IdMacroIndicator)?.Name;
        }
    }
}
