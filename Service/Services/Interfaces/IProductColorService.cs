using Service.ViewModels.ProductColorVms;

namespace Service.Services.Interfaces
{
    public interface IProductColorService
    {
        Task CreateAsync(ProductColorCreateVM model);
    }
}
