using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Setting;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Setting, SettingUpdateVM>();
            CreateMap<SettingUpdateVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();
        }
    }
}

