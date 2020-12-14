using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Admin
{
    public class StadiumViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Founded")]
        [DataType(DataType.Date)]
        public DateTime FoundedOn { get; set; }

        [Required]
        [Range(100, 120000)]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CountriesItems { get; set; }
    }
}
