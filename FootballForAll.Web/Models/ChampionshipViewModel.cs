using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Web.Models
{
    public class ChampionshipViewModel
    {
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Championship type")]
        public string Type { get; set; } // EPL, Serie A, La Liga ...

        [Display(Name = "Season")]
        public string Season { get; set; }

        [Display(Name = "Name")]
        public string Name => $"{Type} - {Season}";

        [Display(Name = "Results table")]
        public List<ClubViewModel> ResultsTable { get; set; }

        public ChampionshipViewModel()
        {
            // TODO: this example info to be deleted, when EFCore and DB are implemented
            Country = "England";
            Type = "English Premier League";
            Season = "2020/21";
            ResultsTable = new List<ClubViewModel>
            {
                new ClubViewModel(this),
                new ClubViewModel(this),
                new ClubViewModel(this)
            };
        }
    }
}
