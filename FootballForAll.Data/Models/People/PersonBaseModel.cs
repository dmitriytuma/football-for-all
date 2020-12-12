using System;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models.People
{
    public class PersonBaseModel : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public Country Country { get; set; }
    }
}
