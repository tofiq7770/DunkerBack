using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Color;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _ColorService;

        public ColorController(IColorService ColorService)
        {
            _ColorService = ColorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _ColorService.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {

            return View(await _ColorService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (await _ColorService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }
            await _ColorService.CreateAsync(request);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var Color = await _ColorService.GetByIdAsync((int)id);

            if (Color == null) return NotFound();

            return View(Color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ColorUpdateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            var Color = await _ColorService.GetByIdAsync((int)id);

            if (Color == null) return NotFound();

            await _ColorService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var Color = await _ColorService.GetByIdAsync((int)id);

            if (Color == null) return NotFound();

            await _ColorService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
