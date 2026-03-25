using Application.Dtos.MacroIndicator;
using Application.Interfaces.Services;
using Application.ViewModels.MacroIndicator;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVestApp.Controllers
{
    public class MacroIndicatorController : Controller
    {
        private readonly IMacroIndicatorService _macroIndicatorService;

        public MacroIndicatorController(IMacroIndicatorService macroIndicatorService)
        {
            _macroIndicatorService = macroIndicatorService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _macroIndicatorService.GetAllWithIncluded();

            var listEntitiesVms = dtos.Select(s => new MacroIndicatorViewModel()
            {
                IdMacroIndicator = s.IdMacroIndicator,
                Name = s.Name,
                Weight = s.Weight,
                IsHighBetter = s.IsHighBetter,
                IndicatorsQuantity = s.IndicatorsQuantity
            }).ToList();

            return View(listEntitiesVms);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var existingMacros = await _macroIndicatorService.GetAll();
            var totalWeight = existingMacros.Sum(x => x.Weight);

            return View("Save", new SaveMacroIndicatorViewModel()
            {
                Name = "",
                Weight = 0,
                IsHighBetter = true,
                RemainingWeight = 1m - totalWeight
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveMacroIndicatorViewModel vm) 
        {
            // Validar nombre unico
            var existingMacros = await _macroIndicatorService.GetAll();
            if (existingMacros.Any(m => m.Name.Equals(vm.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", "Ya existe un macroindicador con este nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            var all = await _macroIndicatorService.GetAll();
            var totalWeight = all.Sum(x => x.Weight);

            if (totalWeight >= 1)
            {
                ModelState.AddModelError("Weight", "Ya no se pueden crear mas MacroIndicadores porque la suma de los pesos es 1.");
                return View("Save", vm);
            }

            if (totalWeight + vm.Weight > 1m)
            {
                ModelState.AddModelError("Weight",
                    $"El peso ingresado excede el limite. Disponible: {1m - totalWeight:F4}");
                return View("Save", vm);
            }

            MacroIndicatorDto dto = new()
            {
                IdMacroIndicator = 0,
                Name = vm.Name,
                Weight = vm.Weight,
                IsHighBetter = vm.IsHighBetter
            };

            await _macroIndicatorService.AddAsync(dto);
            return RedirectToRoute(new { controller = "MacroIndicator", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.editMode = true;
            var dto = await _macroIndicatorService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "MacroIndicator", action = "Index" });
            }

            var existingMacros = await _macroIndicatorService.GetAll();
            var otherMacrosWeight = existingMacros
                .Where(m => m.IdMacroIndicator != id)
                .Sum(m => m.Weight);

            SaveMacroIndicatorViewModel vm = new()
            {
                IdMacroIndicator = dto.IdMacroIndicator,
                Name = dto.Name,
                Weight = dto.Weight,
                IsHighBetter = dto.IsHighBetter,
                IndicatorsQuantity = dto.IndicatorsQuantity,
                RemainingWeight = 1m - otherMacrosWeight  
            };

            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveMacroIndicatorViewModel vm)
        {
            // Validar nombre unico (excluyendo el actual)
            var existingMacros = await _macroIndicatorService.GetAll();
            if (existingMacros.Any(m => m.IdMacroIndicator != vm.IdMacroIndicator &&
                                        m.Name.Equals(vm.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", "Ya existe un macroindicador con este nombre.");
            }

            var otherMacrosWeight = existingMacros
                .Where(m => m.IdMacroIndicator != vm.IdMacroIndicator)
                .Sum(m => m.Weight);

            if (otherMacrosWeight + vm.Weight > 1m)
            {
                ModelState.AddModelError("Weight",
                    $"El peso ingresado excede el limite. Disponible: {1m - otherMacrosWeight:F4}");
            }

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            MacroIndicatorDto dto = new()
            {
                IdMacroIndicator = vm.IdMacroIndicator,
                Name = vm.Name,
                Weight = vm.Weight,
                IsHighBetter = vm.IsHighBetter
            };

            await _macroIndicatorService.UpdateAsync(dto);
            return RedirectToRoute(new { controller = "MacroIndicator", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _macroIndicatorService.GetById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "MacroIndicator", action = "Index" });
            }

            DeleteMacroIndicatorViewModel vm = new()
            {
                IdMacroIndicator = dto.IdMacroIndicator,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMacroIndicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool deleted = await _macroIndicatorService.DeleteAsync(vm.IdMacroIndicator);

            if (!deleted)
            {
                ViewBag.ErrorMessage = "No se puede eliminar el MacroIndicador porque tiene indicadores asociados.";
                return View(vm);
            }

            return RedirectToRoute(new { controller = "MacroIndicator", action = "Index" });
        }
    }
}
