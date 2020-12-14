using System.Collections.Generic;
using System.Threading.Tasks;
using FootballForAll.Data.Models.People;
using FootballForAll.ViewModels.Admin.People;

namespace FootballForAll.Services.Interfaces
{
    public interface IManagerService
    {
        Manager Get(int id, bool toIncludeRelatedData = true);

        IEnumerable<Manager> GetAll(bool toIncludeRelatedData = true);

        Task CreateAsync(ManagerViewModel managerViewModel);

        Task UpdateAsync(ManagerViewModel managerViewModel);

        Task DeleteAsync(int id);

    }
}
