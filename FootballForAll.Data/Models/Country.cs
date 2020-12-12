using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models
{
    public class Country : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string Code { get; set; }
    }
}
