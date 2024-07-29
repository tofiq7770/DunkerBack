using Service.ViewModels.Category;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListVM>> GetAllAsync();
        Task<CategoryUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task<bool> AnyAsync(string name);
        Task UpdateAsync(int id, CategoryUpdateVM model);
        Task DeleteAsync(int id);
    }
}
