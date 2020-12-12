using System;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models
{
    public class Club : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public Stadium HomeStadium { get; set; }

        [Required]
        public DateTime FoundedOn { get; set; }
    }
}
