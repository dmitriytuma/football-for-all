using System;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Data.Models.People
{
    public class Player : PersonBaseModel
    {
        [Required]
        [Range(1, 100)]
        public int Number { get; set; }

        [Required]
        [Range(1, 12)]
        public Position Position { get; set; }

        [Required]
        public Club Club { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Goals { get; set; }

        [Required]
        [Range(0, 1000)]
        public int YellowCards { get; set; }

        [Required]
        [Range(0, 1000)]
        public int RedCards { get; set; }
    }
}
