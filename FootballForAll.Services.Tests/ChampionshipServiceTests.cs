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
    public class ChampionshipServiceTests
    {
        [Fact]
        public async Task SaveAndLoadChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(championship));
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var championshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                FoundedOn = DateTime.Now,
                CountryId = 1,
                CountryName = "Italy",
                Description = "One of the best championships in the world"
            };

            await championshipService.CreateAsync(championshipViewModel);

            var savedChampionship = championshipService.Get(10, false);
            var lastSavedChampionship = championshipService.GetAll().LastOrDefault();

            Assert.Null(savedChampionship);
            Assert.Equal("Serie A", lastSavedChampionship.Name);
            Assert.Equal("Italy", championshipViewModel.CountryName);
            Assert.Equal("One of the best championships in the world", championshipViewModel.Description);
        }

        [Fact]
        public async Task SaveAndLoadChampionshipWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable()); mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = 1,
                Name = championship.Name,
                Country = championship.Country
            }));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var championshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                FoundedOn = DateTime.Now,
                CountryId = 1
            };

            await championshipService.CreateAsync(championshipViewModel);

            var savedChampionship = championshipService.Get(1, true);

            Assert.Equal("Serie A", savedChampionship.Name);
        }

        [Fact]
        public async Task SaveAndLoadChampionshipsWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable()); mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = 1,
                Name = championship.Name,
                Country = championship.Country
            }));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var championshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                FoundedOn = DateTime.Now,
                CountryId = 1
            };

            await championshipService.CreateAsync(championshipViewModel);

            var savedChampionships = championshipService.GetAll();

            Assert.True(savedChampionships.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoChampionshipsWithSameNames()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(championship));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var firstChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1
            };

            var secondChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1
            };

            await championshipService.CreateAsync(firstChampionshipViewModel);

            await Assert.ThrowsAsync<Exception>(() => championshipService.CreateAsync(secondChampionshipViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = 1,
                Name = championship.Name,
                Country = championship.Country
            }));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var championshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1
            };

            await championshipService.CreateAsync(championshipViewModel);

            var updatedViewModel = new ChampionshipViewModel
            {
                Id = 1,
                Name = "Serie A",
                CountryId = 1
            };

            await championshipService.UpdateAsync(updatedViewModel);

            var savedChampionship = championshipService.Get(1);

            Assert.Equal(1, savedChampionship.Id);
            Assert.Equal("Serie A", savedChampionship.Name);
        }

        [Fact]
        public async Task UpdateNotExistingChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var updatedViewModel = new ChampionshipViewModel
            {
                Id = 1,
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => championshipService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateChampionshipWithNameOfAnotherdExistingChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();
            var id = 1;

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = id++,
                Name = championship.Name,
                Country = championship.Country
            }));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var firstChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1
            };

            var secondChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "La Liga",
                CountryId = 1
            };

            await championshipService.CreateAsync(firstChampionshipViewModel);
            await championshipService.CreateAsync(secondChampionshipViewModel);

            var secondUpdatedViewModel = new ChampionshipViewModel
            {
                Id = 2,
                Name = "Serie A",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => championshipService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = 1,
                Name = championship.Name,
                Country = championship.Country
            }));
            mockChampionshipRepo.Setup(r => r.Delete(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Remove(championship));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var championshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1
            };

            await championshipService.CreateAsync(championshipViewModel);
            await championshipService.DeleteAsync(1);

            Assert.Empty(championshipService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingChampionship()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => championshipService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllChampionshipsAsKeyValuePairs()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Italy", Code = "IT" }
            };
            var championshipsList = new List<Championship>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.AddAsync(It.IsAny<Championship>())).Callback<Championship>(championship => championshipsList.Add(new Championship
            {
                Id = 1,
                Name = championship.Name,
                Country = championship.Country
            }));

            var championshipService = new ChampionshipService(mockChampionshipRepo.Object, mockCountryRepo.Object);

            var firstChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "Serie A",
                CountryId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object).GetAllAsKeyValuePairs()
            };

            var secondChampionshipViewModel = new ChampionshipViewModel
            {
                Name = "Bundesliga",
                CountryId = 1
            };

            await championshipService.CreateAsync(firstChampionshipViewModel);
            await championshipService.CreateAsync(secondChampionshipViewModel);

            var keyValuePairs = championshipService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstChampionshipViewModel.CountriesItems.Count() == 1);
        }
    }
}
