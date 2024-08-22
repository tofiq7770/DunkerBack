using Domain.Entities;
using DunkerFinal.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly IBrandService _brandService;
        private readonly ISliderService _sliderService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IProductTagService _tagProductService;
        private readonly IProductColorService _colorProductService;

        public ShopController(IProductService productService,
                              ICategoryService categoryService,
                              AppDbContext context,
                              UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor httpContextAccessor,
                              ISliderService sliderService,
                              IBrandService brandService,
                              IProductColorService colorProductService,
                              IColorService colorService,
                              ITagService tagService,
                              IProductTagService tagProductTagService
                              )
        {
            _context = context;
            _productService = productService;
            _categoryService = categoryService;
            _colorProductService = colorProductService;
            _userManager = userManager;
            _signInManager = signInManager;
            _sliderService = sliderService;
            _tagService = tagService;
            _colorService = colorService;
            _tagProductService = tagProductTagService;
            _brandService = brandService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            ShopVM model = new()
            {
                Products = await _productService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                Tags = await _tagService.GetAllAsync(),
                Baskets = await _context.BasketProducts.ToListAsync(),
            };

            return View(model);
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
                Categories = categories,
                Brands = await _brandService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
            };

            return View(datas);
        }

        public async Task<IActionResult> Sorting(string sort)
        {
            if (_productService == null)
            {
                throw new InvalidOperationException("Product service is not initialized.");
            }

            IEnumerable<Product> products = await _productService.GetAllAsync();
            if (products == null)
            {
                throw new InvalidOperationException("Products could not be retrieved.");
            }

            switch (sort)
            {
                case "SORT BY RATING":
                    products = products.OrderByDescending(m => m.Rating);
                    break;
                case "SORT BY LATEST":
                    products = products.OrderByDescending(m => m.CreatedTime);
                    break;
                case "SORT BY PRICE HIGH TO LOW":
                    products = products.OrderByDescending(m => m.Price);
                    break;
                case "SORT BY PRICE LOW TO HIGH":
                    products = products.OrderBy(m => m.Price);
                    break;
                default:
                    products = products.OrderBy(m => m.Name);
                    break;
            }

            ShopVM model = new()
            {
                Products = products,
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
            };
            if (model == null)
            {
                throw new InvalidOperationException("Model could not be created.");
            }

            return View("Index", model);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (_productService == null)
            {
                throw new InvalidOperationException("Product service is not initialized.");
            }

            IEnumerable<Product> products;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                products = await _productService.GetAllAsync();
                products = products.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                               p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                               p.Brand.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                               p.Category.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                products = await _productService.GetAllAsync();
            }

            ShopVM model = new()
            {
                Products = products,
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
            };

            return View("Index", model);
        }

        public async Task<IActionResult> Filter(IEnumerable<int> categories, IEnumerable<int> brands, IEnumerable<int> colors, int minPrice, int maxPrice)
        {
            var products = await _productService.GetAllAsync();

            if (categories != null && categories.Any())
            {
                products = products.Where(p => categories.Contains(p.CategoryId)).ToList();
            }

            if (brands != null && brands.Any())
            {
                products = products.Where(p => brands.Contains(p.BrandId)).ToList();
            }

            if (colors != null && colors.Any())
            {
                products = products.Where(p => p.ProductColors != null && p.ProductColors.Any(pc => colors.Contains(pc.ColorId))).ToList();
            }

            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();


            ShopVM model = new()
            {
                Products = products, // No need to call ToList() here
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(), // Ensure this is properly populated
                ProductTags = await _context.ProductTags.ToListAsync(),
            };

            // Return the filtered products to the view
            return View("Index", model);
        }


    }
}
