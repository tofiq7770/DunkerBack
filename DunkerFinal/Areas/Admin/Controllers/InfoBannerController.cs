using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.InfoBanner;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class InfoBannerController : Controller
    {
        private readonly IInfoBannerService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;
        public InfoBannerController(IInfoBannerService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
        }

        public async Task<IActionResult> Index()
        {
            var InfoBanners = await _service.GetAllAsync();

            return View(InfoBanners);
        }

        public async Task<IActionResult> Detail(int id)
        {

            return View(await _service.GetByIdAsync(id));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(InfoBannerCreateVM vm)
        {

            if (await _service.AnyAsync(vm.Title))
            {
                ModelState.AddModelError("Title", $"{vm.Title} is already exist!");
                return View(vm);
            }
            var result = await _service.CreateAsync(vm, ModelState, _imagePath);

            if (!result)
                return View(vm);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {

            var result = await _service.DeleteAsync(id, _imagePath);

            if (!result)
                return NotFound();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var result = await _service.GetUpdatedInfoBannerAsync(id);

            if (result is null)
                return NotFound();

            return View(result);

        }
        [HttpPost]
        public async Task<IActionResult> Update(InfoBannerUpdateVM vm)
        {
            var result = await _service.UpdateAsync(vm, ModelState, _imagePath);

            if (result is null)
                return NotFound();
            else if (result is false)
                return View(vm);

            return RedirectToAction("Index");
        }
    }
}
