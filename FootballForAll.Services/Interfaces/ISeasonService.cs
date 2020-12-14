using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface ISeasonService
    {
        Season Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Season> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task CreateAsync(SeasonViewModel seasonViewModel);

        Task UpdateAsync(SeasonViewModel seasonViewModel);

        Task DeleteAsync(int id);

    }
}
