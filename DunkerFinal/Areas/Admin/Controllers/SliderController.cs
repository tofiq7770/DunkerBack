using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Slider;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _SliderService;
        private readonly IWebHostEnvironment _env;
        public SliderController(ISliderService SliderService, IWebHostEnvironment env)
        {
            _env = env;
            _SliderService = SliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _SliderService.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {

            return View(await _SliderService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await request.Image.SaveFileToLocalAsync(path);
            if (await _SliderService.AnyAsync(request.Title))
            {
                ModelState.AddModelError("Title", $"{request.Title} is already exist!");
                return View(request);
            }
            await _SliderService.CreateAsync(request);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var Slider = await _SliderService.GetByIdAsync((int)id);

            if (Slider == null) return NotFound();

            return View(Slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            var Slider = await _SliderService.GetByIdAsync((int)id);

            if (Slider == null) return NotFound();

            await _SliderService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var Slider = await _SliderService.GetByIdAsync((int)id);

            if (Slider == null) return NotFound();

            await _SliderService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
