using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.ViewModels.Brand;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandListVM>> GetAllAsync();
        Task<BrandUpdateVM> GetByIdAsync(int id);
        Task<bool> CreateAsync(BrandCreateVM model, ModelStateDictionary modelState, string imagePath);
        public Task<bool> DeleteAsync(int id, string imagePath);
        public Task<BrandUpdateVM?> GetUpdatedBrandAsync(int id);
        public Task<bool?> UpdateAsync(BrandUpdateVM model, ModelStateDictionary modelState, string imagePath);
        Task<bool> AnyAsync(string title);
        public Task<bool> IsExistAsync(Expression<Func<Brand, bool>> expression);
    }
}
