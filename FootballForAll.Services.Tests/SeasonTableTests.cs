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
    public class TeamPositionServiceTests
    {
        [Fact]
        public async Task SaveAndLoadTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season { Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(teamPosition));
            mockTeamPositionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => teamPositionsList.FirstOrDefault(c => c.Id == id));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
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

            await teamPositionService.CreateAsync(teamPositionViewModel);

            var savedTeamPosition = teamPositionService.Get(10, false);
            var lastSavedTeamPosition = teamPositionService.GetAll().LastOrDefault();

            Assert.Null(savedTeamPosition);
            Assert.Equal("2020/21", teamPositionViewModel.SeasonName);
            Assert.Equal("Manchester United", teamPositionViewModel.ClubName);
            Assert.Equal(50, teamPositionViewModel.Points);
            Assert.Equal(15, teamPositionViewModel.Won);
            Assert.Equal(5, teamPositionViewModel.Drawn);
            Assert.Equal(0, teamPositionViewModel.Lost);
            Assert.Equal(60, teamPositionViewModel.GoalsFor);
            Assert.Equal(10, teamPositionViewModel.GoalsAgainst);
            Assert.NotNull(lastSavedTeamPosition.Season);
            Assert.NotNull(lastSavedTeamPosition.Club);
        }

        [Fact]
        public async Task SaveAndLoadTeamPositionWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season { Id = 1, Name = "2020/21"}
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await teamPositionService.CreateAsync(teamPositionViewModel);

            var savedTeamPosition = teamPositionService.Get(1, true);

            Assert.NotNull(savedTeamPosition);
            Assert.NotNull(savedTeamPosition.Season);
            Assert.NotNull(savedTeamPosition.Club);
        }

        [Fact]
        public async Task SaveAndLoadTeamPositionsWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await teamPositionService.CreateAsync(teamPositionViewModel);

            var savedTeamPositions = teamPositionService.GetAll();

            Assert.True(savedTeamPositions.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoTeamPositionsWithSameClubsAndSeasons()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(teamPosition));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var firstTeamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            var secondTeamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await teamPositionService.CreateAsync(firstTeamPositionViewModel);

            await Assert.ThrowsAsync<Exception>(() => teamPositionService.CreateAsync(secondTeamPositionViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" },
                new Club { Id = 2, Name = "Chelsea" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await teamPositionService.CreateAsync(teamPositionViewModel);

            var updatedViewModel = new TeamPositionViewModel
            {
                Id = 1,
                SeasonId = 1,
                ClubId = 2
            };

            await teamPositionService.UpdateAsync(updatedViewModel);

            var savedTeamPosition = teamPositionService.Get(1);

            Assert.Equal(1, savedTeamPosition.Id);
        }

        [Fact]
        public async Task UpdateNotExistingTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var updatedViewModel = new TeamPositionViewModel
            {
                Id = 1,
                SeasonId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => teamPositionService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateTeamPositionWithDataOfAnotherdExistingTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" },
                new Club { Id = 2, Name = "Chelsea" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();
            var id = 1;

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = id++,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var firstTeamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            var secondTeamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 2
            };

            await teamPositionService.CreateAsync(firstTeamPositionViewModel);
            await teamPositionService.CreateAsync(secondTeamPositionViewModel);

            var secondUpdatedViewModel = new TeamPositionViewModel
            {
                Id = 2,
                SeasonId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => teamPositionService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));
            mockTeamPositionRepo.Setup(r => r.Delete(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Remove(teamPosition));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
            {
                SeasonId = 1,
                ClubId = 1
            };

            await teamPositionService.CreateAsync(teamPositionViewModel);
            await teamPositionService.DeleteAsync(1);

            Assert.Empty(teamPositionService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingTeamPosition()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var seasonsList = new List<Season> {
                new Season{ Id = 1, Name = "2020/21" }
            };
            var teamPositionsList = new List<TeamPosition>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => teamPositionService.DeleteAsync(1));
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
            var teamPositionsList = new List<TeamPosition>();

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

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var teamPositionViewModel = new TeamPositionViewModel
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

            await teamPositionService.CreateAsync(teamPositionViewModel);

            Assert.True(teamPositionViewModel.ClubsItems.Count() == 1);
            Assert.True(teamPositionViewModel.SeasonsItems.Count() == 1);
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
            var teamPositionsList = new List<TeamPosition>();

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

            var mockTeamPositionRepo = new Mock<IRepository<TeamPosition>>();
            mockTeamPositionRepo.Setup(r => r.All()).Returns(teamPositionsList.AsQueryable());
            mockTeamPositionRepo.Setup(r => r.AddAsync(It.IsAny<TeamPosition>())).Callback<TeamPosition>(teamPosition => teamPositionsList.Add(new TeamPosition
            {
                Id = 1,
                Season = teamPosition.Season,
                Club = teamPosition.Club
            }));

            var teamPositionService = new TeamPositionService(mockTeamPositionRepo.Object, mockSeasonRepo.Object, mockClubRepo.Object);

            var positions = teamPositionService.GetChampionshipSeasonPositions(1);

            Assert.NotNull(positions);
        }
    }
}
