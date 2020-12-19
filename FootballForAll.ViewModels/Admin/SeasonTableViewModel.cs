using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.ViewModels.Admin.Common;

namespace FootballForAll.ViewModels.Admin
{
    public class TeamPositionViewModel : BaseViewModel
    {
        [Required]
        [Range(0, 255)]
        [Display(Name = "Points")]
        public byte Points { get; set; }

        [Required]
        [Range(0, 255)]
        [Display(Name = "Won")]
        public byte Won { get; set; }

        [Required]
        [Range(0, 255)]
        [Display(Name = "Drawn")]
        public byte Drawn { get; set; }

        [Required]
        [Range(0, 255)]
        [Display(Name = "Lost")]
        public byte Lost { get; set; }

        [Required]
        [Range(0, 255)]
        [Display(Name = "GF")]
        public byte GoalsFor { get; set; }

        [Required]
        [Display(Name = "GA")]
        [Range(0, 255)]
        public byte GoalsAgainst { get; set; }

        [Required]
        [Display(Name = "Season")]
        public int SeasonId { get; set; }

        [Display(Name = "Season")]
        public string SeasonName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> SeasonsItems { get; set; }

        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }

        [Display(Name = "Club")]
        public string ClubName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ClubsItems { get; set; }
    }
}
