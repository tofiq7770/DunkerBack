namespace Service.Services.Interfaces
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAll();
    }
}
