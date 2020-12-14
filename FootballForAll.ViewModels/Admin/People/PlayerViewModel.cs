using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballForAll.Data.Models.People;

namespace FootballForAll.ViewModels.Admin.People
{
    public class PlayerViewModel : PersonBaseViewModel
    {
        [Required]
        [Range(1, 100)]
        [Display(Name = "Number")]
        public int Number { get; set; }

        [Required]
        [Range(1, 12)]
        [Display(Name = "Position")]
        public Position Position { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Goals")]
        public int Goals { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Yellow cards")]
        public int YellowCards { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Red cards")]
        public int RedCards { get; set; }

        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }

        [Display(Name = "Club")]
        public string ClubName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ClubsItems { get; set; }
    }
}
