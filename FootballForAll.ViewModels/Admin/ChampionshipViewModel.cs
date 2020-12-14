using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class ChampionshipViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Founded")]
        [DataType(DataType.Date)]
        public DateTime FoundedOn { get; set; }

        [MaxLength(400)]
        [Display(Name = "Desription")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CountriesItems { get; set; }
    }
}
