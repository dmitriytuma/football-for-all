using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> countryRepository;

        public CountryService(IRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Country Get(int id)
        {
            return countryRepository.Get(id);
        }

        public IEnumerable<Country> GetAll()
        {
            return countryRepository.All().ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return countryRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync(CountryViewModel countryViewModel)
        {
            var doesCountryExist = countryRepository.All().Any(c => c.Name == countryViewModel.Name || c.Code == countryViewModel.Code);

            if (doesCountryExist)
            {
                throw new Exception($"Country with a name {countryViewModel.Name} or a code {countryViewModel.Code} already exists.");
            }

            var country = new Country
            {
                Name = countryViewModel.Name,
                Code = countryViewModel.Code
            };

            await countryRepository.AddAsync(country);
            await countryRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(CountryViewModel countryViewModel)
        {
            var allCountries = countryRepository.All();
            var country = allCountries.FirstOrDefault(c => c.Id == countryViewModel.Id);

            if (country is null)
            {
                throw new Exception($"Country not found");
            }

            var doesCountryExist = allCountries.Any(c => c.Id != countryViewModel.Id && (c.Name == countryViewModel.Name || c.Code == countryViewModel.Code));

            if (doesCountryExist)
            {
                throw new Exception($"Country with a name {countryViewModel.Name} or a code {countryViewModel.Code} already exists.");
            }

            country.Name = countryViewModel.Name;
            country.Code = countryViewModel.Code;

            await countryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allCountries = countryRepository.All();
            var country = allCountries.FirstOrDefault(c => c.Id == id);

            if (country is null)
            {
                throw new Exception($"Country not found");
            }

            countryRepository.Delete(country);

            await countryRepository.SaveChangesAsync();
        }
    }
}
