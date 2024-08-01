using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int? Rating { get; set; } = 0;
    }
}
