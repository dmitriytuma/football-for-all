using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models.People;
using FootballForAll.ViewModels.Admin.People;

namespace FootballForAll.Services.Interfaces
{
    public interface IPlayerService
    {
        Player Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Player> GetAll(bool toIncludeRelatedData = true);

        Task CreateAsync(PlayerViewModel playerViewModel);

        Task UpdateAsync(PlayerViewModel playerViewModel);

        Task DeleteAsync(int id);

    }
}
