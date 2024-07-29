using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Category;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _CategoryService.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {

            return View(await _CategoryService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (await _CategoryService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }
            await _CategoryService.CreateAsync(request);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var Category = await _CategoryService.GetByIdAsync((int)id);

            if (Category == null) return NotFound();

            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            var Category = await _CategoryService.GetByIdAsync((int)id);

            if (Category == null) return NotFound();

            await _CategoryService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var Category = await _CategoryService.GetByIdAsync((int)id);

            if (Category == null) return NotFound();

            await _CategoryService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}

