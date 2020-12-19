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
    public class TeamPositionService : ITeamPositionService
    {
        private readonly IRepository<TeamPosition> teamPositionRepository;
        private readonly IRepository<Season> seasonRepository;
        private readonly IRepository<Club> clubRepository;

        public TeamPositionService(
            IRepository<TeamPosition> teamPositionRepository,
            IRepository<Season> seasonRepository,
            IRepository<Club> clubRepository)
        {
            this.teamPositionRepository = teamPositionRepository;
            this.seasonRepository = seasonRepository;
            this.clubRepository = clubRepository;
        }

        public TeamPosition Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return teamPositionRepository.All()
                    .Include(s => s.Season)
                    .ThenInclude(s => s.Championship)
                    .Include(s => s.Club)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return teamPositionRepository.Get(id);
            }
        }

        public IEnumerable<TeamPosition> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return teamPositionRepository.All()
                    .Include(s => s.Season)
                    .ThenInclude(s => s.Championship)
                    .Include(s => s.Club)
                    .ToList();
            }
            else
            {
                return teamPositionRepository.All().ToList();
            }
        }

        public IEnumerable<TeamPosition> GetChampionshipSeasonPositions(int id)
        {
            return teamPositionRepository.All()
                .Include(s => s.Season)
                .ThenInclude(s => s.Championship)
                .ThenInclude(c => c.Country)
                .Include(s => s.Club)
                .Where(s => s.Season.Id == id)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalsFor)
                .ToList();
        }

        public async Task CreateAsync(TeamPositionViewModel teamPositionViewModel)
        {
            var doesTeamPositionExist = teamPositionRepository.All()
                .Any(c => c.Season.Id == teamPositionViewModel.SeasonId && c.Club.Id == teamPositionViewModel.ClubId);

            if (doesTeamPositionExist)
            {
                throw new Exception($"Combination of Season and Club already exists.");
            }

            var teamPosition = new TeamPosition
            {
                Points = teamPositionViewModel.Points,
                Won = teamPositionViewModel.Won,
                Drawn = teamPositionViewModel.Drawn,
                Lost = teamPositionViewModel.Lost,
                GoalsFor = teamPositionViewModel.GoalsFor,
                GoalsAgainst = teamPositionViewModel.GoalsAgainst,
                Season = seasonRepository.Get(teamPositionViewModel.SeasonId),
                Club = clubRepository.Get(teamPositionViewModel.ClubId)
            };

            await teamPositionRepository.AddAsync(teamPosition);
            await teamPositionRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeamPositionViewModel teamPositionViewModel)
        {
            var allTeamPositions = teamPositionRepository.All();
            var teamPosition = allTeamPositions.FirstOrDefault(c => c.Id == teamPositionViewModel.Id);

            if (teamPosition is null)
            {
                throw new Exception($"TeamPosition not found");
            }

            var doesTeamPositionExist = allTeamPositions.Any(c =>
                c.Id != teamPositionViewModel.Id &&
                c.Season.Id == teamPositionViewModel.SeasonId &&
                c.Club.Id == teamPositionViewModel.ClubId);

            if (doesTeamPositionExist)
            {
                throw new Exception($"Combination of Season and Club already exists.");
            }

            teamPosition.Points = teamPositionViewModel.Points;
            teamPosition.Won = teamPositionViewModel.Won;
            teamPosition.Drawn = teamPositionViewModel.Drawn;
            teamPosition.Lost = teamPositionViewModel.Lost;
            teamPosition.GoalsFor = teamPositionViewModel.GoalsFor;
            teamPosition.GoalsAgainst = teamPositionViewModel.GoalsAgainst;
            teamPosition.Season = seasonRepository.Get(teamPositionViewModel.SeasonId);
            teamPosition.Club = clubRepository.Get(teamPositionViewModel.ClubId);

            await teamPositionRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allTeamPositions = teamPositionRepository.All();
            var teamPosition = allTeamPositions.FirstOrDefault(c => c.Id == id);

            if (teamPosition is null)
            {
                throw new Exception($"TeamPosition not found");
            }

            teamPositionRepository.Delete(teamPosition);

            await teamPositionRepository.SaveChangesAsync();
        }
    }
}
