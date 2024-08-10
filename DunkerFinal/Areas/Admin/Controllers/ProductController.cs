
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Product;
using Service.ViewModels.ProductColorVms;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {

        private readonly IProductService _service;
        private readonly IColorService _colorService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IBrandService _brandService;
        private readonly IProductColorService _productColorService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagePath;

        public ProductController(IProductService service, IWebHostEnvironment environment, IProductColorService productColorService, IColorService colorService, ICategoryService categoryService, IBrandService brandService, ITagService tagService)
        {
            _service = service;
            _environment = environment;
            _imagePath = Path.Combine(_environment.WebRootPath, "assets", "images");
            _productColorService = productColorService;
            _colorService = colorService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            if (!ModelState.IsValid) return View(request);

            int productId = await _service.CreateAsync(request);

            foreach (var colorId in request.ColorIds)
            {
                await _productColorService.CreateAsync(new ProductColorCreateVM() { ProductId = productId, ColorId = colorId });
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            var result = await _service.GetUpdatedProductAsync(id, ViewBag);

            if (result is null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            var result = await _service.UpdateAsync(vm, ModelState, ViewBag, _imagePath);

            if (result is null)
                return BadRequest();
            else if (result is false)
                return View(vm);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id, _imagePath);

            if (result is false)
                return NotFound();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result is null)
                return NotFound();


            return View(result);

        }
    }
}
