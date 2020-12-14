using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface IClubService
    {
        Club Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Club> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task CreateAsync(ClubViewModel clubViewModel);

        Task UpdateAsync(ClubViewModel clubViewModel);

        Task DeleteAsync(int id);

    }
}
