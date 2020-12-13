using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Main
{
    public class ClubViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Founded")]
        public DateTime FoundedOn { get; set; }

        [Display(Name = "Home stadium")]
        public string HomeStadiumName { get; set; }
    }
}
