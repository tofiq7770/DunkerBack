﻿
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Product;
using Service.ViewModels.ProductColorVms;
using Service.ViewModels.ProductImageVMs;
using Service.ViewModels.ProductTagVMs;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {

        private readonly IProductService _service;
        private readonly IProductImageService _imageService;
        private readonly IColorService _colorService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IBrandService _brandService;
        private readonly IProductColorService _productColorService;
        private readonly IProductTagService _productTagService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagePath;

        public ProductController(IProductService service,
            IWebHostEnvironment environment,
            IProductColorService productColorService,
            IColorService colorService,
            ICategoryService categoryService,
            IBrandService brandService,
            ITagService tagService,
            IProductTagService productTagService, IProductImageService imageService)
        {
            _productTagService = productTagService;
            _service = service;
            _environment = environment;
            _imagePath = Path.Combine(_environment.WebRootPath, "assets", "images");
            _productColorService = productColorService;
            _colorService = colorService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _imageService = imageService;
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

            foreach (var item in request.Images)
            {
                if (!item.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Images", "File must be Image Format");
                    return View(request);
                }

                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Images", "Max File capacity must be 200KB");
                    return View(request);
                }

            }

            int productId = await _service.CreateAsync(request);

            foreach (var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
                await item.SaveFileToLocalAsync(path);

                await _imageService.CreateAsync(new ProductImageCreateVM() { Image = fileName, ProductId = productId, IsMain = false }); ;
            }

            foreach (var colorId in request.ColorIds)
            {
                await _productColorService.CreateAsync(new ProductColorCreateVM() { ProductId = productId, ColorId = colorId });
            }

            foreach (var tagId in request.TagIds)
            {
                await _productTagService.CreateAsync(new ProductTagCreateVM() { ProductId = productId, TagId = tagId });
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