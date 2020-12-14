using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface IMatchService
    {
        Match Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Match> GetAll(bool toIncludeRelatedData = true);

        Task CreateAsync(MatchViewModel matchViewModel);

        Task UpdateAsync(MatchViewModel matchViewModel);

        Task DeleteAsync(int id);

    }
}
