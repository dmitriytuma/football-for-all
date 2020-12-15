using System;

namespace FootballForAll.ViewModels.Main
{
    public class MatchDetailsViewModel
    {
        public int SeasonId { get; set; }

        public string ChampionshipName { get; set; }

        public DateTime PlayedOn { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public string Result => PlayedOn <= DateTime.Now ? $"{HomeTeamGoals} - {AwayTeamGoals}" : "- / -";

        public string MainRefereeName { get; set; }

        public string StadiumName { get; set; }

        public int Attendance { get; set; }

    }
}
