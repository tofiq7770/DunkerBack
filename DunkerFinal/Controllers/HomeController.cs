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
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService,
                              ISliderService sliderService,
                              IBrandService brandService,
                              ICategoryService categoryService,
                              AppDbContext context)
        {

            _context = context;
            _sliderService = sliderService;
            _brandService = brandService;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {

            HomeVM model = new()
            {

                Products = await _productService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Baskets = await _context.BasketProducts.ToListAsync(),
                Sliders = await _sliderService.GetAllAsync()
            };

            return View(model);
        }
    }
}