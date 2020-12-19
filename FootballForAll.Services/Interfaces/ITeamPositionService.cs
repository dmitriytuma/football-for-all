using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface ITeamPositionService
    {
        TeamPosition Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<TeamPosition> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<TeamPosition> GetChampionshipSeasonPositions(int id);

        Task CreateAsync(TeamPositionViewModel teamPositionViewModel);

        Task UpdateAsync(TeamPositionViewModel teamPositionViewModel);

        Task DeleteAsync(int id);

    }
}
