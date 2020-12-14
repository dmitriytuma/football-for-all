using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class SeasonViewModel : BaseViewModel
    {
        [Required]
        [RegularExpression(@"(19|20)(\d){2}-(\d){2}")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [MaxLength(400)]
        [Display(Name = "Desription")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Championship")]
        public int ChampionshipId { get; set; }

        [Display(Name = "Championship")]
        public string ChampionshipName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ChampionshipsItems { get; set; }
    }
}
