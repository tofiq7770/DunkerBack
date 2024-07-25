using DunkerFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace DunkerFinal.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public FooterViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _settingService.GetAll();

            FooterVM model = new()
            {
                Settings = settings
            };

            return await Task.FromResult(View(model));
        }
    }
}
