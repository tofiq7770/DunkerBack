using Domain.Entities;
using DunkerFinal.ViewModels.Header;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Services.Interfaces;

namespace DunkerFinal.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(ISettingService settingService, UserManager<AppUser> userManager, AppDbContext context)
        {
            _settingService = settingService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser existUser = new();

            if (User.Identity.IsAuthenticated)
            {
                existUser = await _userManager.FindByNameAsync(User.Identity?.Name);
            }

            int quantity = await _context.BasketProducts.Where(m => m.Basket.AppUserId == existUser.Id).CountAsync();
            HeaderVM model = new()
            {
                Settings = await _settingService.GetAll(),
                BasketCount = quantity
            };

            return await Task.FromResult(View(model));
        }
    }

}
