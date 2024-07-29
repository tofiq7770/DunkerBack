using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
