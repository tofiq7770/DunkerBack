using Domain.Entities;
using DunkerFinal.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.DAL;
using Service.Services.Interfaces;
using Service.ViewModels.Category;
using Service.ViewModels.Product;

namespace DunkerFinal.Controllers
{
    public class ShopController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShopController(IProductService productService,
                              ICategoryService categoryService,
                              AppDbContext context,
                              UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor httpContextAccessor

                              )
        {
            _context = context;
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductDetail(int? Id)
        {
            List<Product> products = await _productService.GetAllAsync();
            ProductDetailVM product = await _productService.GetByIdAsync((int)Id);
            IEnumerable<CategoryListVM> categories = await _categoryService.GetAllAsync();

            AppUser existUser = new();

            if (User.Identity.IsAuthenticated)
                existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.AppUserId = existUser.Id;

            ShopVM datas = new()
            {
                Products = products,
                Product = product,
                Categories = categories
            };

            return View(datas);
        }
    }
}
