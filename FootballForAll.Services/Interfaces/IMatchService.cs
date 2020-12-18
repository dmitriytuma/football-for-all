using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;
using FootballForAll.ViewModels.Main;

namespace FootballForAll.Services.Interfaces
{
    public interface IMatchService
    {
        Match Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Match> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<FixturesViewModel> GetAllGroupedByDate(DateTime? date);

        Task CreateAsync(MatchViewModel matchViewModel);

        Task UpdateAsync(MatchViewModel matchViewModel);

        Task DeleteAsync(int id);

    }
}
