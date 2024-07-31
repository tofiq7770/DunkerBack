using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Tag;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _TagService;

        public TagController(ITagService TagService)
        {
            _TagService = TagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _TagService.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {

            return View(await _TagService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (await _TagService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }
            await _TagService.CreateAsync(request);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var Tag = await _TagService.GetByIdAsync((int)id);

            if (Tag == null) return NotFound();

            return View(Tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TagUpdateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            var Tag = await _TagService.GetByIdAsync((int)id);

            if (Tag == null) return NotFound();

            await _TagService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var Tag = await _TagService.GetByIdAsync((int)id);

            if (Tag == null) return NotFound();

            await _TagService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}