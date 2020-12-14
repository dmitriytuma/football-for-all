using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models.People;
using FootballForAll.ViewModels.Admin.People;

namespace FootballForAll.Services.Interfaces
{
    public interface IRefereeService
    {
        Referee Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Referee> GetAll(bool toIncludeRelatedData = true);

        Task CreateAsync(RefereeViewModel RefereeViewModel);

        Task UpdateAsync(RefereeViewModel RefereeViewModel);

        Task DeleteAsync(int id);

    }
}
