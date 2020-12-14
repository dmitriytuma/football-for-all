using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class CountryViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        [Display(Name = "Code")]
        public string Code { get; set; }

    }
}
