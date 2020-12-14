using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.People;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin.People;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Services.Implementations
{
    public class ManagerService : IManagerService
    {
        private readonly IRepository<Manager> managerRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Club> clubRepository;

        public ManagerService(
            IRepository<Manager> managerRepository,
            IRepository<Country> countryRepository,
            IRepository<Club> clubRepository)
        {
            this.managerRepository = managerRepository;
            this.countryRepository = countryRepository;
            this.clubRepository = clubRepository;
        }

        public Manager Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return managerRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.Club)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return managerRepository.Get(id);
            }
        }

        public IEnumerable<Manager> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return managerRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.Club)
                    .ToList();
            }
            else
            {
                return managerRepository.All().ToList();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return managerRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(ManagerViewModel managerViewModel)
        {
            var doesManagerExist = managerRepository.All().Any(c => c.Name == managerViewModel.Name);

            if (doesManagerExist)
            {
                throw new Exception($"Manager with a name {managerViewModel.Name} already exists.");
            }

            var manager = new Manager
            {
                Name = managerViewModel.Name,
                BirthDate = managerViewModel.BirthDate,
                Country = countryRepository.Get(managerViewModel.CountryId),
                Club = clubRepository.Get(managerViewModel.ClubId)
            };

            await managerRepository.AddAsync(manager);
            await managerRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ManagerViewModel managerViewModel)
        {
            var allManagers = managerRepository.All();
            var manager = allManagers.FirstOrDefault(c => c.Id == managerViewModel.Id);

            if (manager is null)
            {
                throw new Exception($"Manager not found");
            }

            var doesManagerExist = allManagers.Any(c => c.Id != managerViewModel.Id && c.Name == managerViewModel.Name);

            if (doesManagerExist)
            {
                throw new Exception($"Manager with a name {managerViewModel.Name} already exists.");
            }

            manager.Name = managerViewModel.Name;
            manager.BirthDate = managerViewModel.BirthDate;
            manager.Country = countryRepository.Get(managerViewModel.CountryId);
            manager.Club = clubRepository.Get(managerViewModel.ClubId);

            await managerRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allManagers = managerRepository.All();
            var manager = allManagers.FirstOrDefault(c => c.Id == id);

            if (manager is null)
            {
                throw new Exception($"Manager not found");
            }

            managerRepository.Delete(manager);

            await managerRepository.SaveChangesAsync();
        }
    }
}
