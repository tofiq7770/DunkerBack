using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Setting
{
    public class SettingUpdateVM
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
