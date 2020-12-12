using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Web.Models
{
    public class ClubViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Founded")]
        public DateTime FoundedDate { get; set; }

        [Display(Name = "Fixtures")]
        public List<MatchViewModel> Fixtures { get; set; }

        [Display(Name = "Results")]
        public List<MatchViewModel> Results { get; set; }

        [Display(Name = "Players")]
        public List<string> Players { get; set; }

        public ClubViewModel(ChampionshipViewModel championship)
        {
            // TODO: this example info to be deleted, when EFCore and DB are implemented
            Name = "Manchester United";
            FoundedDate = new DateTime(1878, 1, 1);
            Fixtures = new List<MatchViewModel> { new MatchViewModel(championship, this) };
            Results = new List<MatchViewModel> { new MatchViewModel(championship, this) };
            Players = new List<string>
            {
                "1 - David De Gea",
                "10 - Marcus Rashford",
                "18 - Bruno Fernandesh",
                "29 - Aaron Wan-Bissaka"
            };
        }
    }
}
