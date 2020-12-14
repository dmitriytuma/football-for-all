using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Admin.People
{
    public class ManagerViewModel : PersonBaseViewModel
    {
        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }

        [Display(Name = "Club")]
        public string ClubName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ClubsItems { get; set; }
    }
}
