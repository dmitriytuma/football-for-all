using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface IStadiumService
    {
        Stadium Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Stadium> GetAll(bool toIncludeRelatedData = true);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task CreateAsync(StadiumViewModel stadiumViewModel);

        Task UpdateAsync(StadiumViewModel stadiumViewModel);

        Task DeleteAsync(int id);

    }
}
