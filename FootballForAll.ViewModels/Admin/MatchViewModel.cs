using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class MatchViewModel : BaseViewModel
    {
        [Required]
        [Range(0, 255)]
        [Display(Name = "Home team goals")]
        public byte HomeTeamGoals { get; set; }

        [Required]
        [Range(0, 255)]
        [Display(Name = "Away team goals")]
        public byte AwayTeamGoals { get; set; }

        [Required]
        [Range(0, 120000)]
        [Display(Name = "Attendance")]
        public int Attendance { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Played on")]
        public DateTime PlayedOn { get; set; }

        [Required]
        [Display(Name = "Home team")]
        public int HomeTeamId { get; set; }

        [Display(Name = "Home team")]
        public string HomeTeamName { get; set; }

        [Required]
        [Display(Name = "Away team")]
        public int AwayTeamId { get; set; }

        [Display(Name = "Away team")]
        public string AwayTeamName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ClubsItems { get; set; }


        [Required]
        [Display(Name = "Season")]
        public int SeasonId { get; set; }

        [Display(Name = "Season")]
        public string SeasonName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> SeasonsItems { get; set; }


        [Required]
        [Display(Name = "Stadium")]
        public int StadiumId { get; set; }

        [Display(Name = "Stadium")]
        public string StadiumName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> StadiumsItems { get; set; }


        [Required]
        [Display(Name = "Referee")]
        public int RefereeId { get; set; }

        [Display(Name = "Referee")]
        public string RefereeName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RefereesItems { get; set; }
    }
}
