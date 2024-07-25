using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class SettingService : ISettingService
    {
        public readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _settingRepository.GetAll();
        }

    }
}