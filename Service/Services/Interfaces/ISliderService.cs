using Service.ViewModels.Slider;

namespace Service.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderListVM>> GetAllAsync();
        Task<SliderUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(SliderCreateVM model);
        Task<bool> AnyAsync(string title);
        Task UpdateAsync(int id, SliderUpdateVM model);
        Task DeleteAsync(int id);
    }
}
