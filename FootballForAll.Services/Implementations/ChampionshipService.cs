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
    public class ChampionshipService : IChampionshipService
    {
        private readonly IRepository<Championship> championshipRepository;
        private readonly IRepository<Country> countryRepository;

        public ChampionshipService(IRepository<Championship> championshipRepository, IRepository<Country> countryRepository)
        {
            this.championshipRepository = championshipRepository;
            this.countryRepository = countryRepository;
        }

        public Championship Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return championshipRepository.All()
                    .Include(s => s.Country)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return championshipRepository.Get(id);
            }
        }

        public IEnumerable<Championship> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return championshipRepository.All()
                    .Include(s => s.Country)
                    .ToList();
            }
            else
            {
                return championshipRepository.All().ToList();
            }
        }

        public async Task CreateAsync(ChampionshipViewModel championshipViewModel)
        {
            var doesChampionshipExist = championshipRepository.All().Any(c => c.Name == championshipViewModel.Name);

            if (doesChampionshipExist)
            {
                throw new Exception($"Championship with a name {championshipViewModel.Name} already exists.");
            }

            var championship = new Championship
            {
                Name = championshipViewModel.Name,
                FoundedOn = championshipViewModel.FoundedOn,
                Description = championshipViewModel.Description,
                Country = countryRepository.Get(championshipViewModel.CountryId)
            };

            await championshipRepository.AddAsync(championship);
            await championshipRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChampionshipViewModel championshipViewModel)
        {
            var allChampionships = championshipRepository.All();
            var championship = allChampionships.FirstOrDefault(c => c.Id == championshipViewModel.Id);

            if (championship is null)
            {
                throw new Exception($"Championship not found");
            }

            var doesChampionshipExist = allChampionships.Any(c => c.Id != championshipViewModel.Id && c.Name == championshipViewModel.Name);

            if (doesChampionshipExist)
            {
                throw new Exception($"Championship with a name {championshipViewModel.Name} already exists.");
            }

            championship.Name = championshipViewModel.Name;
            championship.FoundedOn = championshipViewModel.FoundedOn;
            championship.Description = championshipViewModel.Description;
            championship.Country = countryRepository.Get(championshipViewModel.CountryId);

            await championshipRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allChampionships = championshipRepository.All();
            var championship = allChampionships.FirstOrDefault(c => c.Id == id);

            if (championship is null)
            {
                throw new Exception($"Championship not found");
            }

            championshipRepository.Delete(championship);

            await championshipRepository.SaveChangesAsync();
        }
    }
}
