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
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<Player> playerRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Club> clubRepository;

        public PlayerService(
            IRepository<Player> playerRepository,
            IRepository<Country> countryRepository,
            IRepository<Club> stadiumRepository)
        {
            this.playerRepository = playerRepository;
            this.countryRepository = countryRepository;
            this.clubRepository = stadiumRepository;
        }

        public Player Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return playerRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.Club)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return playerRepository.Get(id);
            }
        }

        public IEnumerable<Player> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return playerRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.Club)
                    .ToList();
            }
            else
            {
                return playerRepository.All().ToList();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return playerRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(PlayerViewModel playerViewModel)
        {
            var doesPlayerExist = playerRepository.All().Any(c => c.Name == playerViewModel.Name);

            if (doesPlayerExist)
            {
                throw new Exception($"Player with a name {playerViewModel.Name} already exists.");
            }

            var player = new Player
            {
                Name = playerViewModel.Name,
                BirthDate = playerViewModel.BirthDate,
                Number = playerViewModel.Number,
                Position = playerViewModel.Position,
                Goals = playerViewModel.Goals,
                YellowCards = playerViewModel.YellowCards,
                RedCards = playerViewModel.RedCards,
                Country = countryRepository.Get(playerViewModel.CountryId),
                Club = clubRepository.Get(playerViewModel.ClubId)
            };

            await playerRepository.AddAsync(player);
            await playerRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlayerViewModel playerViewModel)
        {
            var allPlayers = playerRepository.All();
            var player = allPlayers.FirstOrDefault(c => c.Id == playerViewModel.Id);

            if (player is null)
            {
                throw new Exception($"Player not found");
            }

            var doesPlayerExist = allPlayers.Any(c => c.Id != playerViewModel.Id && c.Name == playerViewModel.Name);

            if (doesPlayerExist)
            {
                throw new Exception($"Player with a name {playerViewModel.Name} already exists.");
            }

            player.Name = playerViewModel.Name;
            player.BirthDate = playerViewModel.BirthDate;
            player.Number = playerViewModel.Number;
            player.Position = playerViewModel.Position;
            player.Goals = playerViewModel.Goals;
            player.YellowCards = playerViewModel.YellowCards;
            player.RedCards = playerViewModel.RedCards;
            player.Country = countryRepository.Get(playerViewModel.CountryId);
            player.Club = clubRepository.Get(playerViewModel.ClubId);

            await playerRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allPlayers = playerRepository.All();
            var player = allPlayers.FirstOrDefault(c => c.Id == id);

            if (player is null)
            {
                throw new Exception($"Player not found");
            }

            playerRepository.Delete(player);

            await playerRepository.SaveChangesAsync();
        }
    }
}
