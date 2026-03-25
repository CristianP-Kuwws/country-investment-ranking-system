using Application.Dtos.Country;
using Application.Interfaces.Services;
using Application.Services;
using Application.ViewModels.Country;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HorizonFutureVestApp.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _countryService.GetAllWithIncluded();

            var listEntitiesVms = dtos.Select(s => new CountryViewModel()
            {
                IdCountry = s.IdCountry,
                Name = s.Name,
                ISOCode = s.ISOCode,
                IndicatorsQuantity = s.IndicatorsQuantity
            }).ToList();


            return View(listEntitiesVms);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveCountryViewModel()
            {
                Name = "",
                ISOCode = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCountryViewModel vm)
        {
            // Validar nombre unico
            var existingByName = await _countryService.GetAll();
            if (existingByName.Any(c => c.Name.Equals(vm.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", "Ya existe un pais con este nombre.");
            }

            // Validar ISO unico
            if (existingByName.Any(c => c.ISOCode.Equals(vm.ISOCode, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("ISOCode", "Ya existe un pais con este codigo ISO.");
            }

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            CountryDto dto = new() 
            {
                IdCountry = 0,
                Name = vm.Name,
                ISOCode = vm.ISOCode

            };
            await _countryService.AddAsync(dto);
            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id) 
        {
            ViewBag.editMode = true;
            var dto = await _countryService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Country", action = "Index" });
            }

            SaveCountryViewModel vm = new()
            {
                IdCountry = dto.IdCountry,
                Name = dto.Name,
                ISOCode = dto.ISOCode,
                IndicatorsQuantity = dto.IndicatorsQuantity
            };
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCountryViewModel vm)
        {
            // Validar nombre unico (excluyendo el pais actual)
            var existingByName = await _countryService.GetAll();
            if (existingByName.Any(c => c.IdCountry != vm.IdCountry &&
                                        c.Name.Equals(vm.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", "Ya existe un pais con este nombre.");
            }

            // Validar ISO unico (excluyendo el pais actual)
            if (existingByName.Any(c => c.IdCountry != vm.IdCountry &&
                                        c.ISOCode.Equals(vm.ISOCode, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("ISOCode", "Ya existe un pais con este codigo ISO.");
            }

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            CountryDto dto = new()
            {
                IdCountry = vm.IdCountry,
                Name = vm.Name,
                ISOCode = vm.ISOCode

            };
            await _countryService.UpdateAsync(dto);
            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _countryService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Country", action = "Index" });
            }
            DeleteCountryViewModel vm = new()
            {
                IdCountry = dto.IdCountry,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCountryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool deleted = await _countryService.DeleteAsync(vm.IdCountry);

            if (!deleted)
            {
                ViewBag.ErrorMessage = "No se puede eliminar el pais porque tiene indicadores asociados.";
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }
    }
}
