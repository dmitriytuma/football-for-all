using System.Collections.Generic;

namespace FootballForAll.ViewModels.Main
{
    public class FixturesViewModel
    {
        public FixturesViewModel()
        {
            Matches = new HashSet<MatchBasicInfoViewModel>();
        }

        public int SeasonId { get; set; }

        /// <summary>
        /// Name of the championship
        /// </summary>
        public string ChampionshipName { get; set; }

        /// <summary>
        /// Basic info about matches from the current season of the championship
        /// </summary>
        public ICollection<MatchBasicInfoViewModel> Matches { get; set; }
    }
}
