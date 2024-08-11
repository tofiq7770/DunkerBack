using Domain.Entities;

namespace Service.ViewModels.Product
{
    public class ProductDetailVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string SKU { get; set; }

        public int Rating { get; set; } = 5;

        public int Quantity { get; set; }
        public decimal Weight { get; set; }

        public string Category { get; set; }
        public string Brand { get; set; }


        public List<string> TagIds { get; set; }
        public List<string> ColorIds { get; set; }

        public List<ProductImage> Images { get; set; }
    }
}
