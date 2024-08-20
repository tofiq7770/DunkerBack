using DunkerFinal.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Services.Interfaces;

namespace DunkerFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly IInfoBannerService _infoService;
        private readonly IBannerService _bannerService;
        private readonly ISliderService _sliderService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IProductTagService _tagProductService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService,
                              ISliderService sliderService,
                              IInfoBannerService infoService,
                              IBrandService brandService,
                              IBannerService bannerService,
                              IColorService colorService,
                              ITagService tagService,
                              IProductTagService tagProductTagService,
                              ICategoryService categoryService,
                              AppDbContext context)
        {

            _context = context;
            _sliderService = sliderService;
            _infoService = infoService;
            _bannerService = bannerService;
            _tagService = tagService;
            _colorService = colorService;
            _tagProductService = tagProductTagService;
            _brandService = brandService;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            HomeVM model = new()
            {

                Products = await _productService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                InfoBanner = await _infoService.GetAllAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                Tags = await _tagService.GetAllAsync(),
                Banners = await _bannerService.GetAllAsync(),
                Baskets = await _context.BasketProducts.ToListAsync(),
                Sliders = await _sliderService.GetAllAsync()
            };

            return View(model);
        }
    }
}