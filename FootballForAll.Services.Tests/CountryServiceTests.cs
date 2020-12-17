using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Implementations;
using FootballForAll.ViewModels.Admin;
using Moq;
using Xunit;

namespace FootballForAll.Services.Tests
{
    public class CountryServiceTests
    {
        [Fact]
        public async Task SaveAndLoadCountry()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(country));
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var countryService = new CountryService(mockRepo.Object);

            var countryViewModel = new CountryViewModel
            {
                Name = "Australia",
                Code = "AU"
            };

            await countryService.CreateAsync(countryViewModel);

            var savedCountry = countryService.Get(50);
            var lastSavedCountry = countryService.GetAll().LastOrDefault();

            Assert.Null(savedCountry);
            Assert.Equal("Australia", lastSavedCountry.Name);
            Assert.Equal("AU", lastSavedCountry.Code);
        }

        [Fact]
        public async Task SaveTwoCountriesWithSameNames()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(country));

            var countryService = new CountryService(mockRepo.Object);

            var firstCountryViewModel = new CountryViewModel
            {
                Name = "France",
                Code = "FR"
            };

            var secondCountryViewModel = new CountryViewModel
            {
                Name = "France",
                Code = "FN"
            };

            await countryService.CreateAsync(firstCountryViewModel);

            await Assert.ThrowsAsync<Exception>(() => countryService.CreateAsync(secondCountryViewModel));
        }

        [Fact]
        public async Task SaveTwoCountriesWithSameCodes()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(country));

            var countryService = new CountryService(mockRepo.Object);

            var firstCountryViewModel = new CountryViewModel
            {
                Name = "FirstCountry",
                Code = "BG"
            };

            var secondCountryViewModel = new CountryViewModel
            {
                Name = "SecondCountry",
                Code = "BG"
            };

            await countryService.CreateAsync(firstCountryViewModel);

            await Assert.ThrowsAsync<Exception>(() => countryService.CreateAsync(secondCountryViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateCountry()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(new Country
            {
                Id = 1,
                Name = country.Name,
                Code = country.Code
            }));
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var countryService = new CountryService(mockRepo.Object);

            var countryViewModel = new CountryViewModel
            {
                Name = "Bulgariaaaa",
                Code = "BB"
            };

            await countryService.CreateAsync(countryViewModel);

            var updatedViewModel = new CountryViewModel
            {
                Id = 1,
                Name = "Bulgaria",
                Code = "BG"
            };

            await countryService.UpdateAsync(updatedViewModel);

            var savedCountry = countryService.Get(1);

            Assert.Equal(1, savedCountry.Id);
            Assert.Equal("Bulgaria", savedCountry.Name);
            Assert.Equal("BG", savedCountry.Code);
        }

        [Fact]
        public async Task UpdateNotExistingCountry()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());

            var countryService = new CountryService(mockRepo.Object);

            var updatedViewModel = new CountryViewModel
            {
                Id = 1,
                Name = "Bulgaria",
                Code = "BG"
            };

            await Assert.ThrowsAsync<Exception>(() => countryService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateCountryWithNameOfAnotherdExistingCountry()
        {
            var countriesList = new List<Country>();
            var id = 1;

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(new Country
            {
                Id = id++,
                Name = country.Name,
                Code = country.Code
            }));

            var countryService = new CountryService(mockRepo.Object);

            var firstCountryViewModel = new CountryViewModel
            {
                Name = "Switzerland",
                Code = "SW"
            };

            var secondCountryViewModel = new CountryViewModel
            {
                Name = "Scotland",
                Code = "SC"
            };

            await countryService.CreateAsync(firstCountryViewModel);
            await countryService.CreateAsync(secondCountryViewModel);

            var secondUpdatedViewModel = new CountryViewModel
            {
                Id = 2,
                Name = "Switzerland",
                Code = "SC"
            };

            await Assert.ThrowsAsync<Exception>(() => countryService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteCountry()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(new Country
            {
                Id = 1,
                Name = country.Name,
                Code = country.Code
            }));
            mockRepo.Setup(r => r.Delete(It.IsAny<Country>())).Callback<Country>(country => countriesList.Remove(country));

            var countryService = new CountryService(mockRepo.Object);

            var countryViewModel = new CountryViewModel
            {
                Name = "Norway",
                Code = "NO"
            };

            await countryService.CreateAsync(countryViewModel);
            await countryService.DeleteAsync(1);

            Assert.Empty(countryService.GetAll());
        }

        [Fact]
        public async Task DeleteNotExistingCountry()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.Delete(It.IsAny<Country>())).Callback<Country>(country => countriesList.Remove(country));

            var countryService = new CountryService(mockRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => countryService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllCountriesAsKeyValuePairs()
        {
            var countriesList = new List<Country>();

            var mockRepo = new Mock<IRepository<Country>>();
            mockRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Country>())).Callback<Country>(country => countriesList.Add(new Country
            {
                Id = 1,
                Name = country.Name,
                Code = country.Code
            }));

            var countryService = new CountryService(mockRepo.Object);

            var firstCountryViewModel = new CountryViewModel
            {
                Name = "Romania",
                Code = "RO"
            };

            var secondCountryViewModel = new CountryViewModel
            {
                Name = "Greece",
                Code = "GR"
            };

            await countryService.CreateAsync(firstCountryViewModel);
            await countryService.CreateAsync(secondCountryViewModel);

            var keyValuePairs = countryService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
        }
    }
}
