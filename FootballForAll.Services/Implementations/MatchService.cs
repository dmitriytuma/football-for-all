using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.People;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Services.Implementations
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Club> clubRepository;
        private readonly IRepository<Season> seasonRepository;
        private readonly IRepository<Stadium> stadiumRepository;
        private readonly IRepository<Referee> refereeRepository;

        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<Club> clubRepository,
            IRepository<Season> seasonRepository,
            IRepository<Stadium> stadiumRepository,
            IRepository<Referee> refereeRepository)
        {
            this.matchRepository = matchRepository;
            this.clubRepository = clubRepository;
            this.seasonRepository = seasonRepository;
            this.stadiumRepository = stadiumRepository;
            this.refereeRepository = refereeRepository;
        }

        public Match Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return matchRepository.All()
                    .Include(m => m.HomeTeam)
                    .Include(m => m.AwayTeam)
                    .Include(m => m.Season)
                    .ThenInclude(m => m.Championship)
                    .Include(m => m.Stadium)
                    .Include(m => m.Referee)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return matchRepository.Get(id);
            }
        }

        public IEnumerable<Match> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return matchRepository.All()
                    .Include(m => m.HomeTeam)
                    .Include(m => m.AwayTeam)
                    .Include(m => m.Season)
                    .ThenInclude(m => m.Championship)
                    .Include(m => m.Stadium)
                    .Include(m => m.Referee)
                    .ToList();
            }
            else
            {
                return matchRepository.All().ToList();
            }
        }

        public IEnumerable<IGrouping<Season, Match>> GetAllGroupedByChampionships()
        {
            var matches = matchRepository.All()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Season)
                .ThenInclude(m => m.Championship)
                .Where(m => m.Season.Name.StartsWith(DateTime.Now.Year.ToString()))
                .ToList();

            return matches.GroupBy(m => m.Season);
        }

        public async Task CreateAsync(MatchViewModel matchViewModel)
        {
            var doesMatchExist = matchRepository.All()
                .Any(m =>
                    m.HomeTeam.Id == matchViewModel.HomeTeamId &&
                    m.AwayTeam.Id == matchViewModel.AwayTeamId &&
                    m.Season.Id == matchViewModel.SeasonId);

            if (doesMatchExist)
            {
                throw new Exception("Match between thes two teams and in this season already exists.");
            }

            var match = new Match
            {
                HomeTeamGoals = matchViewModel.HomeTeamGoals,
                AwayTeamGoals = matchViewModel.AwayTeamGoals,
                Attendance = matchViewModel.Attendance,
                PlayedOn = matchViewModel.PlayedOn,
                HomeTeam = clubRepository.Get(matchViewModel.HomeTeamId),
                AwayTeam = clubRepository.Get(matchViewModel.AwayTeamId),
                Season = seasonRepository.Get(matchViewModel.SeasonId),
                Stadium = stadiumRepository.Get(matchViewModel.StadiumId),
                Referee = refereeRepository.Get(matchViewModel.RefereeId)
            };

            await matchRepository.AddAsync(match);
            await matchRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(MatchViewModel matchViewModel)
        {
            var allMatches = matchRepository.All();
            var match = allMatches.FirstOrDefault(c => c.Id == matchViewModel.Id);

            if (match is null)
            {
                throw new Exception($"Match not found");
            }

            var doesMatchExist = allMatches
                .Any(m =>
                    m.Id != matchViewModel.Id &&
                    m.HomeTeam.Id == matchViewModel.HomeTeamId &&
                    m.AwayTeam.Id == matchViewModel.AwayTeamId &&
                    m.Season.Id == matchViewModel.SeasonId);

            if (doesMatchExist)
            {
                throw new Exception($"Combination of Season and Club already exists.");
            }

            match.HomeTeamGoals = matchViewModel.HomeTeamGoals;
            match.AwayTeamGoals = matchViewModel.AwayTeamGoals;
            match.Attendance = matchViewModel.Attendance;
            match.PlayedOn = matchViewModel.PlayedOn;
            match.HomeTeam = clubRepository.Get(matchViewModel.HomeTeamId);
            match.AwayTeam = clubRepository.Get(matchViewModel.AwayTeamId);
            match.Season = seasonRepository.Get(matchViewModel.SeasonId);
            match.Stadium = stadiumRepository.Get(matchViewModel.StadiumId);
            match.Referee = refereeRepository.Get(matchViewModel.RefereeId);

            await matchRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allMatches = matchRepository.All();
            var match = allMatches.FirstOrDefault(c => c.Id == id);

            if (match is null)
            {
                throw new Exception($"Match not found");
            }

            matchRepository.Delete(match);

            await matchRepository.SaveChangesAsync();
        }
    }
}
