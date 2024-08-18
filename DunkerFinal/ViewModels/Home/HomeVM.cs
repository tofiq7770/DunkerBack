using Domain.Entities;
using Service.ViewModels.Brand;
using Service.ViewModels.Category;
using Service.ViewModels.Slider;

namespace DunkerFinal.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<SliderListVM> Sliders { get; set; }
        public IEnumerable<BrandListVM> Brands { get; set; }
        public IEnumerable<BasketProduct> Baskets { get; set; }
        public IEnumerable<CategoryListVM> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
