using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Category;
using Service.ViewModels.Setting;
using Service.ViewModels.Slider;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryUpdateVM, Category>();
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<Category, CategoryVM>();
            CreateMap<Category, CategoryUpdateVM>();
            CreateMap<Category, CategoryListVM>();

            CreateMap<Slider, SliderUpdateVM>();
            CreateMap<Slider, SliderVM>();
            CreateMap<SliderUpdateVM, Slider>();
            CreateMap<Slider, SliderListVM>();
            CreateMap<SliderCreateVM, Slider>();

            CreateMap<Setting, SettingUpdateVM>();
            CreateMap<Setting, SettingVM>();
            CreateMap<SettingUpdateVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();
        }
    }
}

