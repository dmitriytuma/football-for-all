using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface ISeasonTableService
    {
        SeasonTable Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<SeasonTable> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<SeasonTable> GetChampionshipSeasonPositions(int id);

        Task CreateAsync(SeasonTableViewModel seasonTableViewModel);

        Task UpdateAsync(SeasonTableViewModel seasonTableViewModel);

        Task DeleteAsync(int id);

    }
}
