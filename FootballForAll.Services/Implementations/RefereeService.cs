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
    public class RefereeService : IRefereeService
    {
        private readonly IRepository<Referee> refereeRepository;
        private readonly IRepository<Country> countryRepository;

        public RefereeService(IRepository<Referee> refereeRepository, IRepository<Country> countryRepository)
        {
            this.refereeRepository = refereeRepository;
            this.countryRepository = countryRepository;
        }

        public Referee Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return refereeRepository.All()
                    .Include(s => s.Country)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return refereeRepository.Get(id);
            }
        }

        public IEnumerable<Referee> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return refereeRepository.All()
                    .Include(s => s.Country)
                    .ToList();
            }
            else
            {
                return refereeRepository.All().ToList();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return refereeRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(RefereeViewModel refereeViewModel)
        {
            var doesRefereeExist = refereeRepository.All().Any(c => c.Name == refereeViewModel.Name);

            if (doesRefereeExist)
            {
                throw new Exception($"Referee with a name {refereeViewModel.Name} already exists.");
            }

            var referee = new Referee
            {
                Name = refereeViewModel.Name,
                BirthDate = refereeViewModel.BirthDate,
                Country = countryRepository.Get(refereeViewModel.CountryId)
            };

            await refereeRepository.AddAsync(referee);
            await refereeRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefereeViewModel refereeViewModel)
        {
            var allReferees = refereeRepository.All();
            var referee = allReferees.FirstOrDefault(c => c.Id == refereeViewModel.Id);

            if (referee is null)
            {
                throw new Exception($"Referee not found");
            }

            var doesRefereeExist = allReferees.Any(c => c.Id != refereeViewModel.Id && c.Name == refereeViewModel.Name);

            if (doesRefereeExist)
            {
                throw new Exception($"Referee with a name {refereeViewModel.Name} already exists.");
            }

            referee.Name = refereeViewModel.Name;
            referee.BirthDate = refereeViewModel.BirthDate;
            referee.Country = countryRepository.Get(refereeViewModel.CountryId);

            await refereeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allReferees = refereeRepository.All();
            var referee = allReferees.FirstOrDefault(c => c.Id == id);

            if (referee is null)
            {
                throw new Exception($"Referee not found");
            }

            refereeRepository.Delete(referee);

            await refereeRepository.SaveChangesAsync();
        }
    }
}
