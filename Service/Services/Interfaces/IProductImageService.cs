using Service.ViewModels.ProductImageVMs;

namespace Service.Services.Interfaces
{
    public interface IProductImageService
    {
        public Task CreateAsync(ProductImageCreateVM model);
    }
}
