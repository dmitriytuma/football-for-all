using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class ClubViewModel : BaseViewModel
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
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CountriesItems { get; set; }

        [Required]
        public int HomeStadiumId { get; set; }

        [Display(Name = "Home stadium")]
        public string HomeStadiumName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> StadiumsItems { get; set; }
    }
}
