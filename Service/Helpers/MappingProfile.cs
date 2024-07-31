using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Banner;
using Service.ViewModels.Category;
using Service.ViewModels.Setting;
using Service.ViewModels.Slider;
using Service.ViewModels.Tag;

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

            CreateMap<TagUpdateVM, Tag>();
            CreateMap<TagCreateVM, Tag>();
            CreateMap<Tag, TagVM>();
            CreateMap<Tag, TagUpdateVM>();
            CreateMap<Tag, TagListVM>();

            CreateMap<Banner, BannerUpdateVM>();
            CreateMap<Banner, BannerVM>();
            CreateMap<BannerUpdateVM, Banner>();
            CreateMap<Banner, BannerListVM>();
            CreateMap<BannerCreateVM, Banner>();

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

