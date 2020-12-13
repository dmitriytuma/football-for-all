using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Main
{
    public class SeasonViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Championship")]
        public string ChampionshipName { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{ChampionshipName} - {Name}";

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Matches")]
        public ICollection<MatchViewModel> Matches { get; set; }
    }
}
