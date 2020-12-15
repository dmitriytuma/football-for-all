using System.Collections.Generic;

namespace FootballForAll.ViewModels.Main
{
    public class SeasonDetailsViewModel
    {
        public SeasonDetailsViewModel()
        {
            Table = new HashSet<TeamPositionViewModel>();
        }

        public string ChampionshipName { get; set; }

        public string SeasonName { get; set; }

        public string FullName => $"{ChampionshipName} {SeasonName}";

        public string Country { get; set; }

        public ICollection<TeamPositionViewModel> Table { get; set; }
    }
}
