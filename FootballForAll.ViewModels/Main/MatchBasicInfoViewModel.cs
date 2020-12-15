using System;

namespace FootballForAll.ViewModels.Main
{
    public class MatchBasicInfoViewModel
    {
        public int MatchId { get; set; }

        public DateTime PlayedOn { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public string Result => PlayedOn <= DateTime.Now ? $"{HomeTeamGoals} - {AwayTeamGoals}" : "- / -";
    }
}
