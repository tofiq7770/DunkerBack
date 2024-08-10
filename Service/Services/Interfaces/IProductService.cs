using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.ViewModels.Product;


namespace Service.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllAsync(int? categoryId = null);
        public Task SendViewBagElements(dynamic ViewBag);
        public Task<int> CreateAsync(ProductCreateVM model);

        public Task<bool> DeleteAsync(int id, string ImagePath);
        public Task<ProductUpdateVM?> GetUpdatedProductAsync(int id, dynamic ViewBag);
        public Task<bool?> UpdateAsync(ProductUpdateVM vm, ModelStateDictionary ModelState, dynamic ViewBag, string imagePath);
        public Task<Product?> GetByIdAsync(int id);

        public Task<List<Product>> GetBestProducts();
        public Task<List<Product>> GetProductsByCategoryId(int categoryId);
        public Task<List<Product>> GetNewProducts();
        public Task<List<Product>> GetBestSellerProducts();
    }
}
