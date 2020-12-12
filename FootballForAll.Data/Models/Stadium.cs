using System;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models
{
    public class Stadium : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public DateTime FoundedOn { get; set; }

        [Required]
        [Range(100, 120000)]
        public int Capacity { get; set; }
    }
}
