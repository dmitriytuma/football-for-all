using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.People;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Implementations;
using FootballForAll.ViewModels.Admin;
using Moq;
using Xunit;
using Match = FootballForAll.Data.Models.Match;

namespace FootballForAll.Services.Tests
{
    public class MatchServiceTests
    {
        [Fact]
        public async Task SaveAndLoadMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(match));
            mockMatchRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => matchesList.FirstOrDefault(c => c.Id == id));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                StadiumId = 1,
                StadiumName = "Santiago Bernabeu",
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                SeasonId = 1,
                SeasonName = "2020/21",
                RefereeId = 1,
                RefereeName = "Michael Oliver"
            };

            await matchService.CreateAsync(matchViewModel);

            var notExistingMatch = matchService.Get(10, false);
            var savedMatch = matchService.GetAll().LastOrDefault();

            Assert.Null(notExistingMatch);
            Assert.Equal("Real Madrid", matchViewModel.HomeTeamName);
            Assert.Equal("Manchester United", matchViewModel.AwayTeamName);
            Assert.Equal("Santiago Bernabeu", matchViewModel.StadiumName);
            Assert.Equal("2020/21", matchViewModel.SeasonName);
            Assert.Equal("Michael Oliver", matchViewModel.RefereeName);
            Assert.Equal(new DateTime(2020, 1, 1), matchViewModel.PlayedOn);
            Assert.NotNull(savedMatch.HomeTeam);
            Assert.NotNull(savedMatch.AwayTeam);
            Assert.NotNull(savedMatch.Stadium);
            Assert.NotNull(savedMatch.Referee);
            Assert.NotNull(savedMatch.Season);
        }

        [Fact]
        public async Task SaveAndLoadMatchWithRelatedData()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                StadiumId = 1,
                StadiumName = "Santiago Bernabeu",
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                SeasonId = 1,
                SeasonName = "2020/21",
                RefereeId = 1,
                RefereeName = "Michael Oliver"
            };

            await matchService.CreateAsync(matchViewModel);

            var savedMatch = matchService.Get(1, true);

            Assert.NotNull(savedMatch);
            Assert.NotNull(savedMatch.HomeTeam);
            Assert.NotNull(savedMatch.AwayTeam);
            Assert.NotNull(savedMatch.Stadium);
            Assert.NotNull(savedMatch.Season);
            Assert.NotNull(savedMatch.Referee);
        }

        [Fact]
        public async Task SaveAndLoadMatchsWithRelatedData()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                StadiumId = 1,
                StadiumName = "Santiago Bernabeu",
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                SeasonId = 1,
                SeasonName = "2020/21",
                RefereeId = 1,
                RefereeName = "Michael Oliver"
            };

            await matchService.CreateAsync(matchViewModel);

            var savedMatchs = matchService.GetAll();

            Assert.True(savedMatchs.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoSameMatchs()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(match));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var firstMatchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            var secondMatchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await matchService.CreateAsync(firstMatchViewModel);

            await Assert.ThrowsAsync<Exception>(() => matchService.CreateAsync(secondMatchViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await matchService.CreateAsync(matchViewModel);

            var updatedViewModel = new MatchViewModel
            {
                Id = 1,
                HomeTeamId = 2,
                HomeTeamName = "Manchester United",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await matchService.UpdateAsync(updatedViewModel);

            var savedMatch = matchService.Get(1);

            Assert.Equal(1, savedMatch.Id);
            Assert.Equal("Manchester United", savedMatch.HomeTeam.Name);
        }

        [Fact]
        public async Task UpdateNotExistingMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var updatedViewModel = new MatchViewModel
            {
                Id = 1
            };

            await Assert.ThrowsAsync<Exception>(() => matchService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateMatchWithDataOfAnotherdExistingMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var id = 1;

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = id++,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var firstMatchViewModel = new MatchViewModel
            {
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            var secondMatchViewModel = new MatchViewModel
            {
                HomeTeamId = 2,
                HomeTeamName = "Manchester United",
                HomeTeamGoals = 4,
                AwayTeamId = 1,
                AwayTeamName = "Real Madrid",
                AwayTeamGoals = 4,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await matchService.CreateAsync(firstMatchViewModel);
            await matchService.CreateAsync(secondMatchViewModel);

            var secondUpdatedViewModel = new MatchViewModel
            {
                Id = 2,
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await Assert.ThrowsAsync<Exception>(() => matchService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));
            mockMatchRepo.Setup(r => r.Delete(It.IsAny<Match>())).Callback<Match>(match => matchesList.Remove(match));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                Id = 2,
                HomeTeamId = 1,
                HomeTeamName = "Real Madrid",
                HomeTeamGoals = 2,
                AwayTeamId = 2,
                AwayTeamName = "Manchester United",
                AwayTeamGoals = 5,
                SeasonId = 1,
                SeasonName = "2020/21",
            };

            await matchService.CreateAsync(matchViewModel);
            await matchService.DeleteAsync(1);

            Assert.Empty(matchService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingMatch()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21" }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            var mockSeasonRepo = new Mock<IRepository<Season>>();
            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            var mockClubRepo = new Mock<IRepository<Club>>();
            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => matchService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllMatchsAsKeyValuePairs()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var championshipsList = new List<Championship>
            {
                new Championship { Id = 1, Name = "Bundesliga" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21", Championship = championshipsList[0] }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var matchViewModel = new MatchViewModel
            {
                ClubsItems = new ClubService(
                    mockClubRepo.Object,
                    mockCountryRepo.Object,
                    mockStadiumRepo.Object)
                .GetAllAsKeyValuePairs(),
                StadiumsItems = new StadiumService(
                    mockStadiumRepo.Object,
                    mockCountryRepo.Object)
                .GetAllAsKeyValuePairs(),
                SeasonsItems = new SeasonService(
                    mockSeasonRepo.Object,
                    mockChampionshipRepo.Object)
                .GetAllAsKeyValuePairs(),
                RefereesItems = new RefereeService(
                    mockRefereeRepo.Object,
                    mockCountryRepo.Object)
                .GetAllAsKeyValuePairs(),
            };

            await matchService.CreateAsync(matchViewModel);

            Assert.True(matchViewModel.ClubsItems.Count() == 2);
            Assert.True(matchViewModel.StadiumsItems.Count() == 1);
            Assert.True(matchViewModel.SeasonsItems.Count() == 1);
            Assert.True(matchViewModel.RefereesItems.Count() == 1);
        }


        [Fact]
        public void GetAllMatchesGroupedByDate()
        {
            var refereesList = new List<Referee>
            {
                new Referee { Id = 1, Name = "Mihael Oliver" }
            };
            var championshipsList = new List<Championship>
            {
                new Championship { Id = 1, Name = "Premier League" }
            };
            var seasonsList = new List<Season>
            {
                new Season { Id = 1, Name = "2020/21", Championship = championshipsList[0] }
            };
            var stadiumsList = new List<Stadium>
            {
                new Stadium { Id = 1, Name = "Santiago Bernabeu" }
            };
            var clubsList = new List<Club>
            {
                new Club { Id = 1, Name = "Real Madrid" },
                new Club { Id = 2, Name = "Manchester United" }
            };
            var countriesList = new List<Country> {
                new Country { Id = 1, Name = "Spain", Code = "SP" },
                new Country { Id = 2, Name = "England", Code = "EN" }
            };
            var matchesList = new List<Match>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var mockChampionshipRepo = new Mock<IRepository<Championship>>();
            mockChampionshipRepo.Setup(r => r.All()).Returns(championshipsList.AsQueryable());
            mockChampionshipRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => championshipsList.FirstOrDefault(c => c.Id == id));

            var mockSeasonRepo = new Mock<IRepository<Season>>();
            mockSeasonRepo.Setup(r => r.All()).Returns(seasonsList.AsQueryable());
            mockSeasonRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => seasonsList.FirstOrDefault(c => c.Id == id));

            var mockStadiumRepo = new Mock<IRepository<Stadium>>();
            mockStadiumRepo.Setup(r => r.All()).Returns(stadiumsList.AsQueryable());
            mockStadiumRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => stadiumsList.FirstOrDefault(c => c.Id == id));

            var mockClubRepo = new Mock<IRepository<Club>>();
            mockClubRepo.Setup(r => r.All()).Returns(clubsList.AsQueryable());
            mockClubRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => clubsList.FirstOrDefault(c => c.Id == id));

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.All()).Returns(countriesList.AsQueryable());
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockMatchRepo = new Mock<IRepository<Match>>();
            mockMatchRepo.Setup(r => r.All()).Returns(matchesList.AsQueryable());
            mockMatchRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).Callback<Match>(match => matchesList.Add(new Match
            {
                Id = 1,
                HomeTeam = match.HomeTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeam = match.AwayTeam,
                AwayTeamGoals = match.AwayTeamGoals,
                Stadium = match.Stadium,
                Attendance = 20000,
                PlayedOn = new DateTime(2020, 1, 1),
                Season = match.Season,
                Referee = match.Referee
            }));

            var matchService = new MatchService(
                mockMatchRepo.Object,
                mockClubRepo.Object,
                mockSeasonRepo.Object,
                mockStadiumRepo.Object,
                mockRefereeRepo.Object);

            var groupedMathes = matchService.GetAllGroupedByDate(DateTime.Today);

            Assert.NotNull(groupedMathes);
        }
    }
}
