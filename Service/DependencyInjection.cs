using Microsoft.Extensions.DependencyInjection;
using Service.Helpers;
using Service.Services;
using Service.Services.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IInfoBannerService, InfoBannerService>();


            return services;
        }
    }
}
