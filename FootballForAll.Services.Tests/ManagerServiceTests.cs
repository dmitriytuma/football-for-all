using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.People;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Implementations;
using FootballForAll.ViewModels.Admin;
using FootballForAll.ViewModels.Admin.People;
using Moq;
using Xunit;

namespace FootballForAll.Services.Tests
{
    public class ManagerServiceTests
    {
        [Fact]
        public async Task SaveAndLoadManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(manager));
            mockManagerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => managersList.FirstOrDefault(c => c.Id == id));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var managerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                CountryName = "Spain",
                BirthDate = new DateTime(1980, 1, 1),
                ClubId = 1,
                ClubName = "Real Madrid"
            };

            await managerService.CreateAsync(managerViewModel);

            var savedManager = managerService.Get(10, false);
            var lastSavedManager = managerService.GetAll().LastOrDefault();

            Assert.Null(savedManager);
            Assert.Equal("Zinedin Zidan", lastSavedManager.Name);
            Assert.Equal("Spain", lastSavedManager.Country.Name);
            Assert.Equal("Real Madrid", managerViewModel.ClubName);
            Assert.Equal(new DateTime(1980, 1, 1), lastSavedManager.BirthDate);
            Assert.NotNull(lastSavedManager.Country);
            Assert.NotNull(lastSavedManager.Club);
        }

        [Fact]
        public async Task SaveAndLoadManagerWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = 1,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var managerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(managerViewModel);

            var savedManager = managerService.Get(1, true);

            Assert.Equal("Zinedin Zidan", savedManager.Name);
            Assert.NotNull(savedManager.Country);
            Assert.NotNull(savedManager.Club);
        }

        [Fact]
        public async Task SaveAndLoadManagersWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = 1,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var managerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(managerViewModel);

            var savedManagers = managerService.GetAll();

            Assert.True(savedManagers.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoManagersWithSameNames()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(manager));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstManagerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            var secondManagerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(firstManagerViewModel);

            await Assert.ThrowsAsync<Exception>(() => managerService.CreateAsync(secondManagerViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = 1,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var managerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(managerViewModel);

            var updatedViewModel = new ManagerViewModel
            {
                Id = 1,
                Name = "Newcastle United",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.UpdateAsync(updatedViewModel);

            var savedManager = managerService.Get(1);

            Assert.Equal(1, savedManager.Id);
            Assert.Equal("Newcastle United", savedManager.Name);
        }

        [Fact]
        public async Task UpdateNotExistingManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var updatedViewModel = new ManagerViewModel
            {
                Id = 1,
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => managerService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateManagerWithNameOfAnotherdExistingManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();
            var id = 1;

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = id++,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstManagerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            var secondManagerViewModel = new ManagerViewModel
            {
                Name = "Newcastle United",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(firstManagerViewModel);
            await managerService.CreateAsync(secondManagerViewModel);

            var secondUpdatedViewModel = new ManagerViewModel
            {
                Id = 2,
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => managerService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = 1,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));
            mockManagerRepo.Setup(r => r.Delete(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Remove(manager));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var managerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1
            };

            await managerService.CreateAsync(managerViewModel);
            await managerService.DeleteAsync(1);

            Assert.Empty(managerService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingManager()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => managerService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllManagersAsKeyValuePairs()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Real Madrid", Capacity = 76000 }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Spain", Code = "SP" }
            };
            var managersList = new List<Manager>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockManagerRepo = new Mock<IRepository<Manager>>();
            mockManagerRepo.Setup(r => r.All()).Returns(managersList.AsQueryable());
            mockManagerRepo.Setup(r => r.AddAsync(It.IsAny<Manager>())).Callback<Manager>(manager => managersList.Add(new Manager
            {
                Id = 1,
                Name = manager.Name,
                Country = manager.Country,
                Club = manager.Club
            }));

            var managerService = new ManagerService(mockManagerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstManagerViewModel = new ManagerViewModel
            {
                Name = "Zinedin Zidan",
                CountryId = 1,
                ClubId = 1,
                ClubsItems = new ClubService(
                    mockClubRepo.Object,
                    mockCountryRepo.Object,
                    mockStadiumRepo.Object)
                .GetAllAsKeyValuePairs()
            };

            var secondManagerViewModel = new ManagerViewModel
            {
                Name = "Newcastle United",
                CountryId = 1,
                ClubId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object)
                    .GetAllAsKeyValuePairs()
            };

            await managerService.CreateAsync(firstManagerViewModel);
            await managerService.CreateAsync(secondManagerViewModel);

            var keyValuePairs = managerService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstManagerViewModel.ClubsItems.Count() == 1);
            Assert.True(secondManagerViewModel.CountriesItems.Count() == 1);
        }
    }
}
