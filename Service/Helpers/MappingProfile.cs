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
            CreateMap<Category, CategoryUpdateVM>();
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryUpdateVM, Category>();
            CreateMap<Category, CategoryListVM>();
            CreateMap<CategoryCreateVM, Category>();

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

