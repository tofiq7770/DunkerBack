using Domain.Entities;
using Service.ViewModels.Category;
using Service.ViewModels.Product;

namespace DunkerFinal.ViewModels.Shop
{
    public class ShopVM
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public ProductDetailVM Product { get; set; }
        public IEnumerable<CategoryListVM> Categories { get; set; }
    }
}
