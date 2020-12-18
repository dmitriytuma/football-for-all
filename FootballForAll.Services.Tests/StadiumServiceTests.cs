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
    public class StadiumServiceTests
    {
        [Fact]
        public async Task SaveAndLoadStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(stadium));
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var stadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                Capacity = 80000,
                FoundedOn = DateTime.Now,
                CountryId = 1,
                CountryName = "Spain"
            };

            await stadiumService.CreateAsync(stadiumViewModel);

            var savedStadium = stadiumService.Get(10, false);
            var lastSavedStadium = stadiumService.GetAll().LastOrDefault();

            Assert.Null(savedStadium);
            Assert.Equal("Santiago Bernabeu", lastSavedStadium.Name);
            Assert.Equal("Spain", stadiumViewModel.CountryName);
            Assert.Equal(80000, lastSavedStadium.Capacity);
        }

        [Fact]
        public async Task SaveAndLoadStadiumWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable()); mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = 1,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var stadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                Capacity = 80000,
                FoundedOn = DateTime.Now,
                CountryId = 1
            };

            await stadiumService.CreateAsync(stadiumViewModel);

            var savedStadium = stadiumService.Get(1, true);

            Assert.Equal("Santiago Bernabeu", savedStadium.Name);
            Assert.Equal(80000, savedStadium.Capacity);
        }

        [Fact]
        public async Task SaveAndLoadStadiumsWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable()); mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = 1,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var stadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                Capacity = 80000,
                FoundedOn = DateTime.Now,
                CountryId = 1
            };

            await stadiumService.CreateAsync(stadiumViewModel);

            var savedStadiums = stadiumService.GetAll();

            Assert.True(savedStadiums.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoStadiumsWithSameNames()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(stadium));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var firstStadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            var secondStadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await stadiumService.CreateAsync(firstStadiumViewModel);

            await Assert.ThrowsAsync<Exception>(() => stadiumService.CreateAsync(secondStadiumViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = 1,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var stadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await stadiumService.CreateAsync(stadiumViewModel);

            var updatedViewModel = new StadiumViewModel
            {
                Id = 1,
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await stadiumService.UpdateAsync(updatedViewModel);

            var savedStadium = stadiumService.Get(1);

            Assert.Equal(1, savedStadium.Id);
            Assert.Equal("Santiago Bernabeu", savedStadium.Name);
        }

        [Fact]
        public async Task UpdateNotExistingStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var updatedViewModel = new StadiumViewModel
            {
                Id = 1,
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => stadiumService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateStadiumWithNameOfAnotherdExistingStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();
            var id = 1;

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = id++,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var firstStadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            var secondStadiumViewModel = new StadiumViewModel
            {
                Name = "Camp Nou",
                CountryId = 1
            };

            await stadiumService.CreateAsync(firstStadiumViewModel);
            await stadiumService.CreateAsync(secondStadiumViewModel);

            var secondUpdatedViewModel = new StadiumViewModel
            {
                Id = 2,
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => stadiumService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = 1,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));
            mockStadiumRepo.Setup(r => r.Delete(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Remove(stadium));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var stadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1
            };

            await stadiumService.CreateAsync(stadiumViewModel);
            await stadiumService.DeleteAsync(1);

            Assert.Empty(stadiumService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingStadium()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => stadiumService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllStadiumsAsKeyValuePairs()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var stadiumsList = new List<Stadium>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.AddAsync(It.IsAny<Stadium>())).Callback<Stadium>(stadium => stadiumsList.Add(new Stadium
            {
                Id = 1,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                Country = stadium.Country
            }));

            var stadiumService = new StadiumService(mockStadiumRepo.Object, mockCountryRepo.Object);

            var firstStadiumViewModel = new StadiumViewModel
            {
                Name = "Santiago Bernabeu",
                CountryId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object).GetAllAsKeyValuePairs()
            };

            var secondStadiumViewModel = new StadiumViewModel
            {
                Name = "Camp Nou",
                CountryId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object).GetAllAsKeyValuePairs()
            };

            await stadiumService.CreateAsync(firstStadiumViewModel);
            await stadiumService.CreateAsync(secondStadiumViewModel);

            var keyValuePairs = stadiumService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstStadiumViewModel.CountriesItems.Count() == 1);
            Assert.True(secondStadiumViewModel.CountriesItems.Count() == 1);
        }
    }
}
