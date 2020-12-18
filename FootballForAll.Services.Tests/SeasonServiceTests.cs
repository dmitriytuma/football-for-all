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
    public class SeasonServiceTests
    {
        [Fact]
        public async Task SaveAndLoadSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(season));
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var seasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1,
                ChampionshipName = "Premier League",
                Description = "Premier League, Season 2020/21"
            };

            await seasonService.CreateAsync(seasonViewModel);

            var savedSeason = seasonService.Get(10, false);
            var lastSavedSeason = seasonService.GetAll().LastOrDefault();

            Assert.Null(savedSeason);
            Assert.Equal("2020/21", lastSavedSeason.Name);
            Assert.Equal("Premier League, Season 2020/21", seasonViewModel.Description);
            Assert.Equal("Premier League", seasonViewModel.ChampionshipName);
            Assert.NotNull(lastSavedSeason.Championship);
        }

        [Fact]
        public async Task SaveAndLoadSeasonWithRelatedData()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = 1,
                Name = season.Name,
                Championship = season.Championship
            }));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var seasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(seasonViewModel);

            var savedSeason = seasonService.Get(1, true);

            Assert.Equal("2020/21", savedSeason.Name);
            Assert.NotNull(savedSeason.Championship);
        }

        [Fact]
        public async Task SaveAndLoadSeasonsWithRelatedData()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = 1,
                Name = season.Name,
                Championship = season.Championship
            }));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var seasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(seasonViewModel);

            var savedSeasons = seasonService.GetAll();

            Assert.True(savedSeasons.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoSeasonsWithSameNames()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(season));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var firstSeasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            var secondSeasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(firstSeasonViewModel);

            await Assert.ThrowsAsync<Exception>(() => seasonService.CreateAsync(secondSeasonViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = 1,
                Name = season.Name,
                Championship = season.Championship
            }));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var seasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(seasonViewModel);

            var updatedViewModel = new SeasonViewModel
            {
                Id = 1,
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.UpdateAsync(updatedViewModel);

            var savedSeason = seasonService.Get(1);

            Assert.Equal(1, savedSeason.Id);
            Assert.Equal("2020/21", savedSeason.Name);
        }

        [Fact]
        public async Task UpdateNotExistingSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var updatedViewModel = new SeasonViewModel
            {
                Id = 1,
                Name = "2015/16",
                ChampionshipId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => seasonService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateSeasonWithNameOfAnotherdExistingSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();
            var id = 1;

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = id++,
                Name = season.Name,
                Championship = season.Championship
            }));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var firstSeasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            var secondSeasonViewModel = new SeasonViewModel
            {
                Name = "2019/20",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(firstSeasonViewModel);
            await seasonService.CreateAsync(secondSeasonViewModel);

            var secondUpdatedViewModel = new SeasonViewModel
            {
                Id = 2,
                Name = "2020/21",
                ChampionshipId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => seasonService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = 1,
                Name = season.Name,
                Championship = season.Championship
            }));
            mockSeasonRepo.Setup(r => r.Delete(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Remove(season));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var seasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(seasonViewModel);
            await seasonService.DeleteAsync(1);

            Assert.Empty(seasonService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingSeason()
        {
            var championshipsList = new List<Championship> {
                new Championship{ Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => seasonService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllSeasonsAsKeyValuePairs()
        {
            var countriesList = new List<Country>
            {
                new Country { Id = 1, Name = "England" }
            };
            var championshipsList = new List<Championship> {
                new Championship { Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.AddAsync(It.IsAny<Season>())).Callback<Season>(season => seasonsList.Add(new Season
            {
                Id = 1,
                Name = season.Name,
                Championship = season.Championship
            }));

            var seasonService = new SeasonService(mockSeasonRepo.Object, mockChampionshipRepo.Object);

            var firstSeasonViewModel = new SeasonViewModel
            {
                Name = "2020/21",
                ChampionshipId = 1,
                ChampionshipsItems = new ChampionshipService(
                    mockChampionshipRepo.Object,
                    mockCountryRepo.Object)
                    .GetAllAsKeyValuePairs()
            };

            var secondSeasonViewModel = new SeasonViewModel
            {
                Name = "2021/22",
                ChampionshipId = 1
            };

            await seasonService.CreateAsync(firstSeasonViewModel);
            await seasonService.CreateAsync(secondSeasonViewModel);

            var keyValuePairs = seasonService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstSeasonViewModel.ChampionshipsItems.Count() == 1);
        }
    }
}
