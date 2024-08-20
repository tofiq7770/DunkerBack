﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.ViewModels.Wishlist;

namespace DunkerFinal.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _context = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser existUser = new();

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            Wishlist wishlist = await _context.Wishlists?
                .Include(m => m.WishlistProducts)
                .ThenInclude(m => m.Product)
                .ThenInclude(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            List<WishlistListVM> wishlistListVMs = new();

            if (wishlist != null)
            {
                foreach (var item in wishlist.WishlistProducts)
                {
                    wishlistListVMs.Add(new WishlistListVM()
                    {
                        Id = item.Id,
                        Image = item.Product.ProductImages.FirstOrDefault(m => m.IsMain)?.Image,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Quantity = item.Quantity,
                    });
                }
            }
            ViewBag.WishlistProduct = wishlistListVMs;

            return View(wishlistListVMs);
        }
        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] int? productId)
        //{
        //    if (productId is null)
        //        return BadRequest(new { success = false, message = "Product ID is required." });

        //    if (!User.Identity.IsAuthenticated)
        //        return Json(new { success = false, message = "User is not authenticated.", redirectUrl = Url.Action("Login", "Account") });

        //    var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

        //    if (existUser == null)
        //        return Json(new { success = false, message = "User not found." });

        //    var existProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == productId);

        //    if (existProduct is null)
        //        return Json(new { success = false, message = "Product not found." });

        //    var wishlist = await _context.Wishlists
        //        .Include(m => m.WishlistProducts)
        //        .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

        //    var wishlistProduct = await _context.WishlistProducts
        //        .FirstOrDefaultAsync(m => m.ProductId == productId && m.Wishlist.AppUserId == existUser.Id);

        //    if (wishlistProduct != null)
        //    {
        //        wishlistProduct.Quantity++;
        //    }
        //    else
        //    {
        //        if (wishlist == null)
        //        {
        //            wishlist = new Wishlist { AppUserId = existUser.Id };
        //            await _context.Wishlists.AddAsync(wishlist);
        //            await _context.SaveChangesAsync();
        //        }

        //        var newWishlistProduct = new WishlistProduct
        //        {
        //            WishlistId = wishlist.Id,
        //            ProductId = (int)productId,
        //            Quantity = 1
        //        };
        //        await _context.WishlistProducts.AddAsync(newWishlistProduct);
        //    }

        //    await _context.SaveChangesAsync();

        //    int uniqueProductCount = await _context.WishlistProducts
        //        .Where(m => m.Wishlist.AppUserId == existUser.Id)
        //        .CountAsync();

        //    return Json(new { success = true, message = "Product added to wishlist.", uniqueProductCount });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //        return Json(new { success = false, message = "Product ID is missing." });

        //    if (!User.Identity.IsAuthenticated)
        //        return Json(new { success = false, message = "User not authenticated." });

        //    var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
        //    if (existUser == null)
        //        return Json(new { success = false, message = "User not found." });

        //    var basketProduct = await _context.WishlistProducts
        //        .Include(m => m.Product)
        //        .FirstOrDefaultAsync(m => m.Id == id && m.Wishlist.AppUserId == existUser.Id);

        //    if (basketProduct == null)
        //        return Json(new { success = false, message = "Product not found in basket." });

        //    _context.WishlistProducts.Remove(basketProduct);
        //    await _context.SaveChangesAsync();

        //    var totalPrice = await _context.WishlistProducts
        //        .Where(m => m.Wishlist.AppUserId == existUser.Id)
        //        .SumAsync(m => m.Product.Price * m.Quantity);

        //    var totalQuantity = await _context.WishlistProducts
        //        .Where(m => m.Wishlist.AppUserId == existUser.Id)
        //        .SumAsync(m => m.Quantity);

        //    int basketCount = await _context.WishlistProducts
        //        .Where(m => m.Wishlist.AppUserId == existUser.Id)
        //        .CountAsync();

        //    return Json(new
        //    {
        //        success = true,
        //        totalPrice,
        //        totalQuantity,
        //        basketCount,
        //        isEmpty = totalQuantity == 0
        //    });
        //}

        //public class IncreaseRequest
        //{
        //    public int Id { get; set; }
        //}

        //public class ProductRequest
        //{
        //    public int Id { get; set; }
        //}

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] int? productId)
        {
            if (productId is null)
                return BadRequest(new { success = false, message = "Product ID is required." });

            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User is not authenticated.", redirectUrl = Url.Action("Login", "Account") });

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return Json(new { success = false, message = "User not found." });

            var existProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == productId);

            if (existProduct is null)
                return Json(new { success = false, message = "Product not found." });

            var wishlist = await _context.Wishlists
                .Include(m => m.WishlistProducts)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            var wishlistProduct = await _context.WishlistProducts
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.Wishlist.AppUserId == existUser.Id);

            string action;
            if (wishlistProduct != null)
            {
                wishlistProduct.Quantity++;
                action = "added";
            }
            else
            {
                if (wishlist == null)
                {
                    wishlist = new Wishlist { AppUserId = existUser.Id };
                    await _context.Wishlists.AddAsync(wishlist);
                    await _context.SaveChangesAsync();
                }

                var newWishlistProduct = new WishlistProduct
                {
                    WishlistId = wishlist.Id,
                    ProductId = (int)productId,
                    Quantity = 1
                };
                await _context.WishlistProducts.AddAsync(newWishlistProduct);
                action = "added";
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product added to wishlist.", action });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWishlist([FromBody] int? productId)
        {
            if (productId is null)
                return BadRequest(new { success = false, message = "Product ID is required." });

            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User is not authenticated.", redirectUrl = Url.Action("Login", "Account") });

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return Json(new { success = false, message = "User not found." });

            var existProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == productId);

            if (existProduct is null)
                return Json(new { success = false, message = "Product not found." });

            var wishlist = await _context.Wishlists
                .Include(m => m.WishlistProducts)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            var wishlistProduct = await _context.WishlistProducts
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.Wishlist.AppUserId == existUser.Id);

            if (wishlistProduct != null)
            {
                _context.WishlistProducts.Remove(wishlistProduct);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product removed from wishlist.", action = "removed" });
            }
            else
            {
                if (wishlist == null)
                {
                    wishlist = new Wishlist { AppUserId = existUser.Id };
                    await _context.Wishlists.AddAsync(wishlist);
                    await _context.SaveChangesAsync();
                }

                var newWishlistProduct = new WishlistProduct
                {
                    WishlistId = wishlist.Id,
                    ProductId = (int)productId,
                    Quantity = 1
                };
                await _context.WishlistProducts.AddAsync(newWishlistProduct);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product added to wishlist.", action = "added" });
            }
        }

    }
}
