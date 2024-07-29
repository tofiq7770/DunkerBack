using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Category;
using Service.ViewModels.Setting;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryUpdateVM>();
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryUpdateVM, Category>();
            CreateMap<Category, CategoryListVM>();
            CreateMap<CategoryCreateVM, Category>();


            CreateMap<Setting, SettingUpdateVM>();
            CreateMap<Setting, SettingVM>();
            CreateMap<SettingUpdateVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();

        }
    }
}

