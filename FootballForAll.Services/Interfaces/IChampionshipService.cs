using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface IChampionshipService
    {
        Championship Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Championship> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task CreateAsync(ChampionshipViewModel championshipViewModel);

        Task UpdateAsync(ChampionshipViewModel championshipViewModel);

        Task DeleteAsync(int id);

    }
}
