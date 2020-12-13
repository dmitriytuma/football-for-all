using System.ComponentModel.DataAnnotations;

namespace FootballForAll.ViewModels.Main
{
    public class ChampionshipViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; } // TODO: instead of type string - type CountryViewModel?

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
