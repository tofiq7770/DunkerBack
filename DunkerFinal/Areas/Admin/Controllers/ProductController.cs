﻿
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
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

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
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
            AppDbContext context,
            IProductTagService productTagService, IProductImageService imageService, IMapper mapper)
        {
            _productTagService = productTagService;
            _service = service;
            _environment = environment;
            _imagePath = Path.Combine(_environment.WebRootPath, "assets", "images");
            _productColorService = productColorService;
            _colorService = colorService;
            _categoryService = categoryService;
            _brandService = brandService;
            _context = context;
            _tagService = tagService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
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

            for (int i = 0; i < request.Images.Length; i++)
            {
                bool isMain = false;

                if (i == 0) isMain = true;

                string fileName = Guid.NewGuid().ToString() + "-" + request.Images[i].FileName;
                string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
                await request.Images[i].SaveFileToLocalAsync(path);

                await _imageService.CreateAsync(new ProductImageCreateVM() { Image = fileName, ProductId = productId, IsMain = isMain });
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
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.ColorsByProductId = await _productColorService.GetAllSelectListByProductIdAsync(id);
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            var result = await _service.GetByIdAsync(id);

            var model = _mapper.Map<ProductUpdateVM>(result);
            model.Colors = await _productColorService.GetAllByProductIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateVM request)
        {
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.ColorsByProductId = await _productColorService.GetAllSelectListByProductIdAsync(id);
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            await _service.UpdateAsync(request);

            foreach (var colorId in request.ColorIds)
            {
                await _productColorService.CreateAsync(new ProductColorCreateVM() { ProductId = id, ColorId = colorId });
            }

            foreach (var tagId in request.TagIds)
            {
                await _productTagService.CreateAsync(new ProductTagCreateVM() { ProductId = id, TagId = tagId });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var a = await _productColorService.GetByIdAsync(id);
            await _productColorService.Delete(a);
            return Ok();
        }

        public async Task<IActionResult> Delete(int id)
        {
            //if (id <= 0) return BadRequest();

            //var product = await _service.GetByIdAsync(id);

            //if (product is null) return NotFound();


            //foreach (var image in product.ProductImages)
            //{
            //    image.Image.DeleteFile(_imagePath);
            //}

            //await _service.DeleteAsync(id, _imagePath);

            if (id <= 0) return BadRequest();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            foreach (ProductImage image in product.ProductImages)
            {
                image.Image.DeleteFile(_environment.WebRootPath, "assets/img");
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.Colors = await _colorService.GetByIdAsync((int)id);
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            if (id == null) return BadRequest();
            Product product = await _context.Products.Include(m => m.ProductColors).ThenInclude(m => m.Color).Include(m => m.ProductTags).ThenInclude(m => m.Tag).Include(m => m.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            List<ProductImageVM> productImages = new();
            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });

            }
            Product model = new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ProductImages = product.ProductImages.ToList(),
                Brand = product.Brand,
                ProductColors = product.ProductColors,
                ProductTags = product.ProductTags.ToList(),
                Weight = product.Weight

            };
            return View(model);

        }
    }
}
