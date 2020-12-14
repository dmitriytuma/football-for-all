using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin.People
{
    public class PersonBaseViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "First and Last Names")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CountriesItems { get; set; }
    }
}
