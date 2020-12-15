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
    public class SeasonTableService : ISeasonTableService
    {
        private readonly IRepository<SeasonTable> seasonTableRepository;
        private readonly IRepository<Season> seasonRepository;
        private readonly IRepository<Club> clubRepository;

        public SeasonTableService(
            IRepository<SeasonTable> seasonTableRepository,
            IRepository<Season> seasonRepository,
            IRepository<Club> clubRepository)
        {
            this.seasonTableRepository = seasonTableRepository;
            this.seasonRepository = seasonRepository;
            this.clubRepository = clubRepository;
        }

        public SeasonTable Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return seasonTableRepository.All()
                    .Include(s => s.Season)
                    .ThenInclude(s => s.Championship)
                    .Include(s => s.Club)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return seasonTableRepository.Get(id);
            }
        }

        public IEnumerable<SeasonTable> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return seasonTableRepository.All()
                    .Include(s => s.Season)
                    .ThenInclude(s => s.Championship)
                    .Include(s => s.Club)
                    .ToList();
            }
            else
            {
                return seasonTableRepository.All().ToList();
            }
        }

        public IEnumerable<SeasonTable> GetChampionshipSeasonPositions(int id)
        {
            return seasonTableRepository.All()
                .Include(s => s.Season)
                .ThenInclude(s => s.Championship)
                .ThenInclude(c => c.Country)
                .Include(s => s.Club)
                .Where(s => s.Season.Id == id)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalsFor)
                .ToList();
        }

        public async Task CreateAsync(SeasonTableViewModel seasonTableViewModel)
        {
            var doesSeasonTableExist = seasonTableRepository.All()
                .Any(c => c.Season.Id == seasonTableViewModel.SeasonId && c.Club.Id == seasonTableViewModel.ClubId);

            if (doesSeasonTableExist)
            {
                throw new Exception($"Combination of Season and Club already exists.");
            }

            var seasonTable = new SeasonTable
            {
                Points = seasonTableViewModel.Points,
                Won = seasonTableViewModel.Won,
                Drawn = seasonTableViewModel.Drawn,
                Lost = seasonTableViewModel.Lost,
                GoalsFor = seasonTableViewModel.GoalsFor,
                GoalsAgainst = seasonTableViewModel.GoalsAgainst,
                Season = seasonRepository.Get(seasonTableViewModel.SeasonId),
                Club = clubRepository.Get(seasonTableViewModel.ClubId)
            };

            await seasonTableRepository.AddAsync(seasonTable);
            await seasonTableRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(SeasonTableViewModel seasonTableViewModel)
        {
            var allSeasonTables = seasonTableRepository.All();
            var seasonTable = allSeasonTables.FirstOrDefault(c => c.Id == seasonTableViewModel.Id);

            if (seasonTable is null)
            {
                throw new Exception($"SeasonTable not found");
            }

            var doesSeasonTableExist = allSeasonTables.Any(c =>
                c.Id != seasonTableViewModel.Id &&
                c.Season.Id == seasonTableViewModel.SeasonId &&
                c.Club.Id == seasonTableViewModel.ClubId);

            if (doesSeasonTableExist)
            {
                throw new Exception($"Combination of Season and Club already exists.");
            }

            seasonTable.Points = seasonTableViewModel.Points;
            seasonTable.Won = seasonTableViewModel.Won;
            seasonTable.Drawn = seasonTableViewModel.Drawn;
            seasonTable.Lost = seasonTableViewModel.Lost;
            seasonTable.GoalsFor = seasonTableViewModel.GoalsFor;
            seasonTable.GoalsAgainst = seasonTableViewModel.GoalsAgainst;
            seasonTable.Season = seasonRepository.Get(seasonTableViewModel.SeasonId);
            seasonTable.Club = clubRepository.Get(seasonTableViewModel.ClubId);

            await seasonTableRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allSeasonTables = seasonTableRepository.All();
            var seasonTable = allSeasonTables.FirstOrDefault(c => c.Id == id);

            if (seasonTable is null)
            {
                throw new Exception($"SeasonTable not found");
            }

            seasonTableRepository.Delete(seasonTable);

            await seasonTableRepository.SaveChangesAsync();
        }
    }
}
