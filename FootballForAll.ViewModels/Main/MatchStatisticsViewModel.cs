using System;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Main
{
    public class MatchStatisticsViewModel
    {
        [Display(Name = "Season")]
        public SeasonStatisticsViewModel Season { get; set; }

        [Display(Name = "Home team")]
        public string HomeTeamName { get; set; }

        [Display(Name = "Away team")]
        public string AwayTeamName { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        [Display(Name = "Result")]
        public string Result => $"{HomeTeamGoals} - {AwayTeamGoals}";

        [Display(Name = "Stadium")]
        public string StadiumName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime PlayedOn { get; set; }

        [Display(Name = "Main Referee")]
        public string MainRefereeName { get; set; }

        [Display(Name = "Attendance")]
        public int Attendance { get; set; }

    }
}
