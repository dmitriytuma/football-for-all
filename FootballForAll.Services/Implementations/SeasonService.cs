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
    public class SeasonService : ISeasonService
    {
        private readonly IRepository<Season> seasonRepository;
        private readonly IRepository<Championship> championshipRepository;

        public SeasonService(IRepository<Season> seasonRepository, IRepository<Championship> championshipRepository)
        {
            this.seasonRepository = seasonRepository;
            this.championshipRepository = championshipRepository;
        }

        public Season Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return seasonRepository.All()
                    .Include(s => s.Championship)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return seasonRepository.Get(id);
            }
        }

        public IEnumerable<Season> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return seasonRepository.All()
                    .Include(s => s.Championship)
                    .ToList();
            }
            else
            {
                return seasonRepository.All().ToList();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return GetAll(true)
                .Select(x => new
                {
                    x.Id,
                    Name = $"{x.Championship.Name} / {x.Name}",
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(SeasonViewModel seasonViewModel)
        {
            var doesSeasonExist = seasonRepository.All()
                .Any(c => c.Name == seasonViewModel.Name && c.Championship.Id == seasonViewModel.ChampionshipId);

            if (doesSeasonExist)
            {
                throw new Exception($"Season with a name {seasonViewModel.Name} already exists.");
            }

            var season = new Season
            {
                Name = seasonViewModel.Name,
                Description = seasonViewModel.Description,
                Championship = championshipRepository.Get(seasonViewModel.ChampionshipId)
            };

            await seasonRepository.AddAsync(season);
            await seasonRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(SeasonViewModel seasonViewModel)
        {
            var allSeasons = seasonRepository.All();
            var season = allSeasons.FirstOrDefault(c => c.Id == seasonViewModel.Id);

            if (season is null)
            {
                throw new Exception($"Season not found");
            }

            var doesSeasonExist = allSeasons.Any(c =>
                c.Id != seasonViewModel.Id &&
                c.Name == seasonViewModel.Name &&
                c.Championship.Id == seasonViewModel.ChampionshipId);

            if (doesSeasonExist)
            {
                throw new Exception($"Season with a name {seasonViewModel.Name} already exists.");
            }

            season.Name = seasonViewModel.Name;
            season.Description = seasonViewModel.Description;
            season.Championship = championshipRepository.Get(seasonViewModel.ChampionshipId);

            await seasonRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allSeasons = seasonRepository.All();
            var season = allSeasons.FirstOrDefault(c => c.Id == id);

            if (season is null)
            {
                throw new Exception($"Season not found");
            }

            seasonRepository.Delete(season);

            await seasonRepository.SaveChangesAsync();
        }
    }
}
