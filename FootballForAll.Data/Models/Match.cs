using System;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;
using FootballForAll.Data.Models.People;

namespace FootballForAll.Data.Models
{
    public class Match : BaseModel
    {
        [Required]
        public Club HomeTeam { get; set; }

        [Required]
        public Club AwayTeam { get; set; }

        [Required]
        [Range(0, 255)]
        public byte HomeTeamGoals { get; set; }

        [Required]
        [Range(0, 255)]
        public byte AwayTeamGoals { get; set; }

        [Required]
        public Season Season { get; set; }

        [Required]
        public Stadium Stadium { get; set; }

        [Required]
        public Referee Referee { get; set; }

        [Required]
        public DateTime PlayedOn { get; set; }

        [Required]
        [Range(0, 120000)]
        public int Attendance { get; set; }
    }
}
