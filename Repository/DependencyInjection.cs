﻿using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IInfoBannerRepository, InfoBannerRepository>();

            return services;
        }
    }
}
