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
    public class SeasonTableServiceTests
    {
        [Fact]
        public async Task SaveAndLoadSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season { Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(seasonTable));
            mockSeasonTableRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonTablesList.FirstOrDefault(c => c.Id == id));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                ClubId = 1,
                ClubName = "Manchester United",
                SeasonId = 1,
                SeasonName = "2020/21",
                Points = 50,
                Won = 15,
                Drawn = 5,
                Lost = 0,
                GoalsFor = 60,
                GoalsAgainst = 10
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);

            var savedSeasonTable = seasonTableService.Get(10, false);
            var lastSavedSeasonTable = seasonTableService.GetAll().LastOrDefault();

            Assert.Null(savedSeasonTable);
            Assert.Equal("2020/21", seasonTableViewModel.SeasonName);
            Assert.Equal("Manchester United", seasonTableViewModel.ClubName);
            Assert.Equal(50, seasonTableViewModel.Points);
            Assert.Equal(15, seasonTableViewModel.Won);
            Assert.Equal(5, seasonTableViewModel.Drawn);
            Assert.Equal(0, seasonTableViewModel.Lost);
            Assert.Equal(60, seasonTableViewModel.GoalsFor);
            Assert.Equal(10, seasonTableViewModel.GoalsAgainst);
            Assert.NotNull(lastSavedSeasonTable.Season);
            Assert.NotNull(lastSavedSeasonTable.Club);
        }

        [Fact]
        public async Task SaveAndLoadSeasonTableWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season { Id = 1, Name = "2020/21"}
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);

            var savedSeasonTable = seasonTableService.Get(1, true);

            Assert.NotNull(savedSeasonTable);
            Assert.NotNull(savedSeasonTable.Season);
            Assert.NotNull(savedSeasonTable.Club);
        }

        [Fact]
        public async Task SaveAndLoadSeasonTablesWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);

            var savedSeasonTables = seasonTableService.GetAll();

            Assert.True(savedSeasonTables.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoSeasonTablesWithSameClubsAndSeasons()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(seasonTable));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var firstSeasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            var secondSeasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await seasonTableService.CreateAsync(firstSeasonTableViewModel);

            await Assert.ThrowsAsync<Exception>(() => seasonTableService.CreateAsync(secondSeasonTableViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" },
                new Club { Id = 2, Name = "Chelsea" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);

            var updatedViewModel = new SeasonTableViewModel
            {
                Id = 1,
                SeasonId = 1,
                ClubId = 2
            };

            await seasonTableService.UpdateAsync(updatedViewModel);

            var savedSeasonTable = seasonTableService.Get(1);

            Assert.Equal(1, savedSeasonTable.Id);
        }

        [Fact]
        public async Task UpdateNotExistingSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var updatedViewModel = new SeasonTableViewModel
            {
                Id = 1,
                SeasonId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => seasonTableService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateSeasonTableWithDataOfAnotherdExistingSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" },
                new Club { Id = 2, Name = "Chelsea" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();
            var id = 1;

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = id++,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var firstSeasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            var secondSeasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 2
            };

            await seasonTableService.CreateAsync(firstSeasonTableViewModel);
            await seasonTableService.CreateAsync(secondSeasonTableViewModel);

            var secondUpdatedViewModel = new SeasonTableViewModel
            {
                Id = 2,
                SeasonId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => seasonTableService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));
            mockSeasonTableRepo.Setup(r => r.Delete(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Remove(seasonTable));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);
            await seasonTableService.DeleteAsync(1);

            Assert.Empty(seasonTableService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingSeasonTable()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => seasonTableService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllRelatedDataAsKeyValuePairs()
        {
            var championshipList = new List<Championship>
            {
                new Championship { Id = 1, Name = "Premier League" }
            };
            var countriesList = new List<Country>
            {
                new Country { Id = 1, Name = "England", Code = "EN" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21", Championship = championshipList[0] }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonId = 1,
                ClubId = 1,
                ClubsItems = new ClubService(
                    mockClubRepo.Object,
                    mockCountryRepo.Object,
                    mockStadiumRepo.Object)
                    .GetAllAsKeyValuePairs(),
                SeasonsItems = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object)
                    .GetAllAsKeyValuePairs()
            };

            await seasonTableService.CreateAsync(seasonTableViewModel);

            Assert.True(seasonTableViewModel.ClubsItems.Count() == 1);
            Assert.True(seasonTableViewModel.SeasonsItems.Count() == 1);
        }


        [Fact]
        public async Task GetChampionshipSeasonPositions()
        {
            var championshipList = new List<Championship>
            {
                new Championship { Id = 1, Name = "Premier League" }
            };
            var countriesList = new List<Country>
            {
                new Country { Id = 1, Name = "England", Code = "EN" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Old Trafford", Capacity = 76000 }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21", Championship = championshipList[0] }
            };
            var seasonTablesList = new List<SeasonTable>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonTableRepo = new Mock<IRepository<SeasonTable>>();
            mockSeasonTableRepo.Setup(r => r.All()).Returns(seasonTablesList.AsQueryable());
            mockSeasonTableRepo.Setup(r => r.AddAsync(It.IsAny<SeasonTable>())).Callback<SeasonTable>(seasonTable => seasonTablesList.Add(new SeasonTable
            {
                Id = 1,
                Season = seasonTable.Season,
                Club = seasonTable.Club
            }));

            var seasonTableService = new SeasonTableService(mockSeasonTableRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var positions = seasonTableService.GetChampionshipSeasonPositions(1);

            Assert.NotNull(positions);
        }
    }
}
