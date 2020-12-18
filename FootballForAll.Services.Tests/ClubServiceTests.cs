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
    public class ClubServiceTests
    {
        [Fact]
        public async Task SaveAndLoadClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(club));
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var clubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1,
                FoundedOn = DateTime.Now,
                CountryName = "England",
                HomeStadiumName = "Old Trafford"
            };

            await clubService.CreateAsync(clubViewModel);

            var savedClub = clubService.Get(10, false);
            var lastSavedClub = clubService.GetAll().LastOrDefault();

            Assert.Null(savedClub);
            Assert.Equal("Manchester United", lastSavedClub.Name);
            Assert.Equal("England", clubViewModel.CountryName);
            Assert.Equal("Old Trafford", clubViewModel.HomeStadiumName);
            Assert.NotNull(lastSavedClub.Country);
            Assert.NotNull(lastSavedClub.HomeStadium);
        }

        [Fact]
        public async Task SaveAndLoadClubWithRelatedData()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = 1,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var clubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(clubViewModel);

            var savedClub = clubService.Get(1, true);

            Assert.Equal("Manchester United", savedClub.Name);
            Assert.NotNull(savedClub.Country);
            Assert.NotNull(savedClub.HomeStadium);
        }

        [Fact]
        public async Task SaveAndLoadClubsWithRelatedData()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = 1,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var clubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(clubViewModel);

            var savedClubs = clubService.GetAll();

            Assert.True(savedClubs.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoClubsWithSameNames()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(club));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var firstClubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            var secondClubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(firstClubViewModel);

            await Assert.ThrowsAsync<Exception>(() => clubService.CreateAsync(secondClubViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = 1,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var clubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(clubViewModel);

            var updatedViewModel = new ClubViewModel
            {
                Id = 1,
                Name = "Newcastle United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.UpdateAsync(updatedViewModel);

            var savedClub = clubService.Get(1);

            Assert.Equal(1, savedClub.Id);
            Assert.Equal("Newcastle United", savedClub.Name);
        }

        [Fact]
        public async Task UpdateNotExistingClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var updatedViewModel = new ClubViewModel
            {
                Id = 1,
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => clubService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateClubWithNameOfAnotherdExistingClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();
            var id = 1;

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = id++,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var firstClubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            var secondClubViewModel = new ClubViewModel
            {
                Name = "Newcastle United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(firstClubViewModel);
            await clubService.CreateAsync(secondClubViewModel);

            var secondUpdatedViewModel = new ClubViewModel
            {
                Id = 2,
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => clubService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = 1,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));
            mockClubRepo.Setup(r => r.Delete(It.IsAny<Club>())).Callback<Club>(club => clubsList.Remove(club));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var clubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1
            };

            await clubService.CreateAsync(clubViewModel);
            await clubService.DeleteAsync(1);

            Assert.Empty(clubService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingClub()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => clubService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllClubsAsKeyValuePairs()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var clubsList = new List<Club>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.AddAsync(It.IsAny<Club>())).Callback<Club>(club => clubsList.Add(new Club
            {
                Id = 1,
                Name = club.Name,
                Country = club.Country,
                HomeStadium = club.HomeStadium
            }));

            var clubService = new ClubService(mockClubRepo.Object, mockCountryRepo.Object, mockStadiumRepo.Object);

            var firstClubViewModel = new ClubViewModel
            {
                Name = "Manchester United",
                CountryId = 1,
                HomeStadiumId = 1,
                StadiumsItems = new StadiumService(
                    mockStadiumRepo.Object,
                    mockCountryRepo.Object)
                .GetAllAsKeyValuePairs()
            };

            var secondClubViewModel = new ClubViewModel
            {
                Name = "Newcastle United",
                CountryId = 1,
                HomeStadiumId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object)
                    .GetAllAsKeyValuePairs()
            };

            await clubService.CreateAsync(firstClubViewModel);
            await clubService.CreateAsync(secondClubViewModel);

            var keyValuePairs = clubService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstClubViewModel.StadiumsItems.Count() == 1);
            Assert.True(secondClubViewModel.CountriesItems.Count() == 1);
        }
    }
}
