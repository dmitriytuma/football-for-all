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
    public class PlayerServiceTests
    {
        [Fact]
        public async Task SaveAndLoadPlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(player));
            mockPlayerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => playersList.FirstOrDefault(c => c.Id == id));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var playerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                CountryName = "Portugal",
                ClubId = 1,
                ClubName = "Manchester United",
                BirthDate = new DateTime(1980, 1, 1),
                Position = Position.AttackingMidfield,
                Number = 18,
                Goals = 100,
                YellowCards = 10,
                RedCards = 0
            };

            await playerService.CreateAsync(playerViewModel);

            var savedPlayer = playerService.Get(10, false);
            var lastSavedPlayer = playerService.GetAll().LastOrDefault();

            Assert.Null(savedPlayer);
            Assert.Equal("Bruno Fernandesh", lastSavedPlayer.Name);
            Assert.Equal("Portugal", lastSavedPlayer.Country.Name);
            Assert.Equal("Manchester United", playerViewModel.ClubName);
            Assert.Equal(new DateTime(1980, 1, 1), lastSavedPlayer.BirthDate);
            Assert.Equal(Position.AttackingMidfield, playerViewModel.Position);
            Assert.Equal(18, playerViewModel.Number);
            Assert.Equal(100, playerViewModel.Goals);
            Assert.Equal(10, playerViewModel.YellowCards);
            Assert.Equal(0, playerViewModel.RedCards);
            Assert.NotNull(lastSavedPlayer.Country);
            Assert.NotNull(lastSavedPlayer.Club);
        }

        [Fact]
        public async Task SaveAndLoadPlayerWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = 1,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var playerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(playerViewModel);

            var savedPlayer = playerService.Get(1, true);

            Assert.Equal("Bruno Fernandesh", savedPlayer.Name);
            Assert.NotNull(savedPlayer.Country);
            Assert.NotNull(savedPlayer.Club);
        }

        [Fact]
        public async Task SaveAndLoadPlayersWithRelatedData()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = 1,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var playerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(playerViewModel);

            var savedPlayers = playerService.GetAll();

            Assert.True(savedPlayers.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoPlayersWithSameNames()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(player));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstPlayerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            var secondPlayerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(firstPlayerViewModel);

            await Assert.ThrowsAsync<Exception>(() => playerService.CreateAsync(secondPlayerViewModel));
        }

        [Fact]
        public async Task SaveAndUpdatePlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = 1,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var playerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(playerViewModel);

            var updatedViewModel = new PlayerViewModel
            {
                Id = 1,
                Name = "Cristiano Ronaldo",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.UpdateAsync(updatedViewModel);

            var savedPlayer = playerService.Get(1);

            Assert.Equal(1, savedPlayer.Id);
            Assert.Equal("Cristiano Ronaldo", savedPlayer.Name);
        }

        [Fact]
        public async Task UpdateNotExistingPlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var updatedViewModel = new PlayerViewModel
            {
                Id = 1,
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => playerService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdatePlayerWithNameOfAnotherdExistingPlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();
            var id = 1;

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = id++,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstPlayerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            var secondPlayerViewModel = new PlayerViewModel
            {
                Name = "Cristiano Ronaldo",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(firstPlayerViewModel);
            await playerService.CreateAsync(secondPlayerViewModel);

            var secondUpdatedViewModel = new PlayerViewModel
            {
                Id = 2,
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => playerService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeletePlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = 1,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));
            mockPlayerRepo.Setup(r => r.Delete(It.IsAny<Player>())).Callback<Player>(player => playersList.Remove(player));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var playerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1
            };

            await playerService.CreateAsync(playerViewModel);
            await playerService.DeleteAsync(1);

            Assert.Empty(playerService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingPlayer()
        {
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => playerService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllPlayersAsKeyValuePairs()
        {
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Manchester United", Capacity = 76000 }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "Portugal", Code = "PT" }
            };
            var playersList = new List<Player>();

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockPlayerRepo = new Mock<IRepository<Player>>();
            mockPlayerRepo.Setup(r => r.All()).Returns(playersList.AsQueryable());
            mockPlayerRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Callback<Player>(player => playersList.Add(new Player
            {
                Id = 1,
                Name = player.Name,
                Country = player.Country,
                Club = player.Club
            }));

            var playerService = new PlayerService(mockPlayerRepo.Object, mockCountryRepo.Object, mockClubRepo.Object);

            var firstPlayerViewModel = new PlayerViewModel
            {
                Name = "Bruno Fernandesh",
                CountryId = 1,
                ClubId = 1,
                ClubsItems = new ClubService(
                    mockClubRepo.Object,
                    mockCountryRepo.Object,
                    mockStadiumRepo.Object)
                .GetAllAsKeyValuePairs()
            };

            var secondPlayerViewModel = new PlayerViewModel
            {
                Name = "Cristiano Ronaldo",
                CountryId = 1,
                ClubId = 1,
                CountriesItems = new CountryService(mockCountryRepo.Object)
                    .GetAllAsKeyValuePairs()
            };

            await playerService.CreateAsync(firstPlayerViewModel);
            await playerService.CreateAsync(secondPlayerViewModel);

            var keyValuePairs = playerService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
            Assert.True(firstPlayerViewModel.ClubsItems.Count() == 1);
            Assert.True(secondPlayerViewModel.CountriesItems.Count() == 1);
        }
    }
}
