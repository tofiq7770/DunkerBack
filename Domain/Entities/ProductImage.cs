using Domain.Common;

namespace Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsMain { get; set; } = false;
        public string Path { get; set; } = null!;
        public bool IsHover { get; set; } = false;
    }
}
