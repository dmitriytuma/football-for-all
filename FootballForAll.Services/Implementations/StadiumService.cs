using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Services.Implementations
{
    public class StadiumService : IStadiumService
    {
        private readonly IRepository<Stadium> stadiumRepository;
        private readonly IRepository<Country> countryRepository;

        public StadiumService(IRepository<Stadium> stadiumRepository, IRepository<Country> countryRepository)
        {
            this.stadiumRepository = stadiumRepository;
            this.countryRepository = countryRepository;
        }

        public Stadium Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return stadiumRepository.All()
                    .Include(s => s.Country)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return stadiumRepository.Get(id);
            }
        }

        public IEnumerable<Stadium> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return stadiumRepository.All()
                    .Include(s => s.Country)
                    .ToList();
            }
            else
            {
                return stadiumRepository.All().ToList();
            }
        }

        public async Task CreateAsync(StadiumViewModel stadiumViewModel)
        {
            var doesStadiumExist = stadiumRepository.All().Any(c => c.Name == stadiumViewModel.Name);

            if (doesStadiumExist)
            {
                throw new Exception($"Stadium with a name {stadiumViewModel.Name} already exists.");
            }

            var stadium = new Stadium
            {
                Name = stadiumViewModel.Name,
                FoundedOn = stadiumViewModel.FoundedOn,
                Capacity = stadiumViewModel.Capacity,
                Country = countryRepository.Get(stadiumViewModel.CountryId)
            };

            await stadiumRepository.AddAsync(stadium);
            await stadiumRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(StadiumViewModel stadiumViewModel)
        {
            var allStadiums = stadiumRepository.All();
            var stadium = allStadiums.FirstOrDefault(c => c.Id == stadiumViewModel.Id);

            if (stadium is null)
            {
                throw new Exception($"Stadium not found");
            }

            var doesStadiumExist = allStadiums.Any(c => c.Id != stadiumViewModel.Id && c.Name == stadiumViewModel.Name);

            if (doesStadiumExist)
            {
                throw new Exception($"Stadium with a name {stadiumViewModel.Name} already exists.");
            }

            stadium.Name = stadiumViewModel.Name;
            stadium.FoundedOn = stadiumViewModel.FoundedOn;
            stadium.Capacity = stadiumViewModel.Capacity;
            stadium.Country = countryRepository.Get(stadiumViewModel.CountryId);

            await stadiumRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allStadiums = stadiumRepository.All();
            var stadium = allStadiums.FirstOrDefault(c => c.Id == id);

            if (stadium is null)
            {
                throw new Exception($"Stadium not found");
            }

            stadiumRepository.Delete(stadium);

            await stadiumRepository.SaveChangesAsync();
        }
    }
}
