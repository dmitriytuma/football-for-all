using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.Common;

namespace FootballForAll.Data.Models
{
    public class Season : BaseModel
    {
        [Required]
        public Championship Championship { get; set; }

        [Required]
        [RegularExpression(@"(19|20)(\d){2}-(\d){2}")]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
    }
}
