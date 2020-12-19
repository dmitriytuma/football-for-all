using System;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models
{
    public class TeamPosition : BaseModel
    {
        [Required]
        public Season Season { get; set; }

        [Required]
        public Club Club { get; set; }

        [Required]
        [Range(0, 255)]
        public byte Points { get; set; }

        [Required]
        [Range(0, 255)]
        public byte Won { get; set; }

        [Required]
        [Range(0, 255)]
        public byte Drawn { get; set; }

        [Required]
        [Range(0, 255)]
        public byte Lost { get; set; }

        [Required]
        [Range(0, 255)]
        public byte GoalsFor { get; set; }

        [Required]
        [Range(0, 255)]
        public byte GoalsAgainst { get; set; }
    }
}
