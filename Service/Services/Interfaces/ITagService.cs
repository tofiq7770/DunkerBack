using Service.ViewModels.Tag;

namespace Service.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagListVM>> GetAllAsync();
        Task<bool> AnyAsync(string name);
        Task<TagUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(TagCreateVM model);
        Task UpdateAsync(int id, TagUpdateVM model);
        Task DeleteAsync(int id);
    }
}
