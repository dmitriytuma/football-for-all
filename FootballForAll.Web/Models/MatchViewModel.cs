using System;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Web.Models
{
    public class MatchViewModel
    {
        [Display(Name = "Championship")]
        public ChampionshipViewModel Championship { get; set; }

        [Display(Name = "Home team")]
        public ClubViewModel HomeTeam { get; set; }

        [Display(Name = "Away team")]
        public ClubViewModel AwayTeam { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        [Display(Name = "Result")]
        public string Result => $"{HomeTeamGoals} - {AwayTeamGoals}";

        [Display(Name = "Stadium")]
        public string Stadium { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Main Referee")]
        public string MainReferee { get; set; }

        [Display(Name = "Attendance")]
        public int Attendance { get; set; }

        public MatchViewModel(ChampionshipViewModel championship, ClubViewModel club)
        {
            // TODO: this example info to be deleted, when EFCore and DB are implemented
            Championship = championship;
            HomeTeam = club;
            AwayTeam = club;
            HomeTeamGoals = 0;
            AwayTeamGoals = 0;
            Stadium = "Old Trafford";
            Date = DateTime.Now;
            MainReferee = "Mark Clattenburg";
            Attendance = 50;
        }
    }
}
