using Application.Dtos.ReturnRate;
using Application.Interfaces.Services;
using Application.ViewModels.ReturnRate;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVestApp.Controllers
{
    public class ReturnRateController : Controller
    {
        private readonly IReturnRateService _returnRateService;

        public ReturnRateController(IReturnRateService returnRateService)
        {
            _returnRateService = returnRateService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _returnRateService.GetAll();
            var dto = dtos.FirstOrDefault();

            if (dto == null)
            {
                ViewBag.EditMode = false;
                return View("Save", new SaveReturnRateViewModel());
            }

            ViewBag.EditMode = true;
            SaveReturnRateViewModel vm = new()
            {
                IdReturnRate = dto.IdReturnRate,
                MinReturnRate = dto.MinReturnRate,
                MaxReturnRate = dto.MaxReturnRate
            };

            return View("Save", vm); 
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {
            var dtos = await _returnRateService.GetAll();
            var dto = dtos.FirstOrDefault();

            if (dto == null)
            {
                ViewBag.EditMode = false;
                return View(new SaveReturnRateViewModel());
            }

            ViewBag.EditMode = true;
            SaveReturnRateViewModel vm = new()
            {
                IdReturnRate = dto.IdReturnRate,
                MinReturnRate = dto.MinReturnRate,
                MaxReturnRate = dto.MaxReturnRate
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveReturnRateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = vm.IdReturnRate != 0;
                return View(vm);
            }

            if (vm.MinReturnRate >= vm.MaxReturnRate)
            {
                ModelState.AddModelError("", "La tasa minima debe ser menor que la tasa maxima.");
                ViewBag.EditMode = vm.IdReturnRate != 0;
                return View(vm);
            }

            ReturnRateDto dto = new()
            {
                IdReturnRate = vm.IdReturnRate,
                MinReturnRate = vm.MinReturnRate,
                MaxReturnRate = vm.MaxReturnRate
            };

            if (vm.IdReturnRate == 0)
                await _returnRateService.AddAsync(dto);
            else
                await _returnRateService.UpdateAsync(dto);

            TempData["SuccessMessage"] = "Configuracion de tasa de retorno guardada correctamente.";
            return RedirectToAction("Index"); 
        }
    }
}
