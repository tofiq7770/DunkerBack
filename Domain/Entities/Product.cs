using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string SKU { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        public int? Rating { get; set; } = 0;
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
