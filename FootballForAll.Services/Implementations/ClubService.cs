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
    public class ClubService : IClubService
    {
        private readonly IRepository<Club> clubRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Stadium> stadiumRepository;

        public ClubService(
            IRepository<Club> clubRepository,
            IRepository<Country> countryRepository,
            IRepository<Stadium> stadiumRepository)
        {
            this.clubRepository = clubRepository;
            this.countryRepository = countryRepository;
            this.stadiumRepository = stadiumRepository;
        }

        public Club Get(int id, bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return clubRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.HomeStadium)
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return clubRepository.Get(id);
            }
        }

        public IEnumerable<Club> GetAll(bool toIncludeRelatedData = true)
        {
            if (toIncludeRelatedData)
            {
                return clubRepository.All()
                    .Include(s => s.Country)
                    .Include(s => s.HomeStadium)
                    .ToList();
            }
            else
            {
                return clubRepository.All().ToList();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return clubRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(ClubViewModel clubViewModel)
        {
            var doesClubExist = clubRepository.All().Any(c => c.Name == clubViewModel.Name);

            if (doesClubExist)
            {
                throw new Exception($"Club with a name {clubViewModel.Name} already exists.");
            }

            var club = new Club
            {
                Name = clubViewModel.Name,
                FoundedOn = clubViewModel.FoundedOn,
                Country = countryRepository.Get(clubViewModel.CountryId),
                HomeStadium = stadiumRepository.Get(clubViewModel.HomeStadiumId)
            };

            await clubRepository.AddAsync(club);
            await clubRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClubViewModel clubViewModel)
        {
            var allClubs = clubRepository.All();
            var club = allClubs.FirstOrDefault(c => c.Id == clubViewModel.Id);

            if (club is null)
            {
                throw new Exception($"Club not found");
            }

            var doesClubExist = allClubs.Any(c => c.Id != clubViewModel.Id && c.Name == clubViewModel.Name);

            if (doesClubExist)
            {
                throw new Exception($"Club with a name {clubViewModel.Name} already exists.");
            }

            club.Name = clubViewModel.Name;
            club.FoundedOn = clubViewModel.FoundedOn;
            club.Country = countryRepository.Get(clubViewModel.CountryId);
            club.HomeStadium = stadiumRepository.Get(clubViewModel.HomeStadiumId);

            await clubRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allClubs = clubRepository.All();
            var club = allClubs.FirstOrDefault(c => c.Id == id);

            if (club is null)
            {
                throw new Exception($"Club not found");
            }

            clubRepository.Delete(club);

            await clubRepository.SaveChangesAsync();
        }
    }
}
