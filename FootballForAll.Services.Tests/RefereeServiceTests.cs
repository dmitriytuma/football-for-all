using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.People;
using FootballForAll.Data.Repositories;
using FootballForAll.Services.Implementations;
using FootballForAll.ViewModels.Admin.People;
using Moq;
using Xunit;

namespace FootballForAll.Services.Tests
{
    public class RefereeServiceTests
    {
        [Fact]
        public async Task SaveAndLoadReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(referee));
            mockRefereeRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => refereesList.FirstOrDefault(c => c.Id == id));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var refereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.CreateAsync(refereeViewModel);

            var savedReferee = refereeService.Get(10, false);
            var lastSavedReferee = refereeService.GetAll().LastOrDefault();

            Assert.Null(savedReferee);
            Assert.Equal("Michael Oliver", lastSavedReferee.Name);
            Assert.NotNull(lastSavedReferee.Country);
        }

        [Fact]
        public async Task SaveAndLoadRefereeWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = 1,
                Name = referee.Name,
                Country = referee.Country
            }));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var refereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.CreateAsync(refereeViewModel);

            var savedReferee = refereeService.Get(1, true);

            Assert.Equal("Michael Oliver", savedReferee.Name);
            Assert.NotNull(savedReferee.Country);
        }

        [Fact]
        public async Task SaveAndLoadRefereesWithRelatedData()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = 1,
                Name = referee.Name,
                Country = referee.Country
            }));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var refereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.CreateAsync(refereeViewModel);

            var savedReferees = refereeService.GetAll();

            Assert.True(savedReferees.Count() == 1);
        }

        [Fact]
        public async Task SaveTwoRefereesWithSameNames()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(referee));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var firstRefereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            var secondRefereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.CreateAsync(firstRefereeViewModel);

            await Assert.ThrowsAsync<Exception>(() => refereeService.CreateAsync(secondRefereeViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = 1,
                Name = referee.Name,
                Country = referee.Country
            }));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var refereeViewModel = new RefereeViewModel
            {
                Name = "Mike Dean",
                CountryId = 1
            };

            await refereeService.CreateAsync(refereeViewModel);

            var updatedViewModel = new RefereeViewModel
            {
                Id = 1,
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.UpdateAsync(updatedViewModel);

            var savedReferee = refereeService.Get(1);

            Assert.Equal(1, savedReferee.Id);
            Assert.Equal("Michael Oliver", savedReferee.Name);
        }

        [Fact]
        public async Task UpdateNotExistingReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var updatedViewModel = new RefereeViewModel
            {
                Id = 1,
                Name = "Mike Dean",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => refereeService.UpdateAsync(updatedViewModel));
        }

        [Fact]
        public async Task SaveAndUpdateRefereeWithNameOfAnotherdExistingReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();
            var id = 1;

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = id++,
                Name = referee.Name,
                Country = referee.Country
            }));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var firstRefereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            var secondRefereeViewModel = new RefereeViewModel
            {
                Name = "Mike Dean",
                CountryId = 1
            };

            await refereeService.CreateAsync(firstRefereeViewModel);
            await refereeService.CreateAsync(secondRefereeViewModel);

            var secondUpdatedViewModel = new RefereeViewModel
            {
                Id = 2,
                Name = "Michael Oliver",
                CountryId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => refereeService.UpdateAsync(secondUpdatedViewModel));
        }

        [Fact]
        public async Task SaveAndDeleteReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = 1,
                Name = referee.Name,
                Country = referee.Country
            }));
            mockRefereeRepo.Setup(r => r.Delete(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Remove(referee));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var refereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            await refereeService.CreateAsync(refereeViewModel);
            await refereeService.DeleteAsync(1);

            Assert.Empty(refereeService.GetAll(false));
        }

        [Fact]
        public async Task DeleteNotExistingReferee()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            await Assert.ThrowsAsync<Exception>(() => refereeService.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllRefereesAsKeyValuePairs()
        {
            var countriesList = new List<Country> {
                new Country{ Id = 1, Name = "England", Code = "EN" }
            };
            var refereesList = new List<Referee>();

            var mockCountryRepo = new Mock<IRepository<Country>>();
            mockCountryRepo.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => countriesList.FirstOrDefault(c => c.Id == id));

            var mockRefereeRepo = new Mock<IRepository<Referee>>();
            mockRefereeRepo.Setup(r => r.All()).Returns(refereesList.AsQueryable());
            mockRefereeRepo.Setup(r => r.AddAsync(It.IsAny<Referee>())).Callback<Referee>(referee => refereesList.Add(new Referee
            {
                Id = 1,
                Name = referee.Name,
                Country = referee.Country
            }));

            var refereeService = new RefereeService(mockRefereeRepo.Object, mockCountryRepo.Object);

            var firstRefereeViewModel = new RefereeViewModel
            {
                Name = "Michael Oliver",
                CountryId = 1
            };

            var secondRefereeViewModel = new RefereeViewModel
            {
                Name = "Mike Dean",
                CountryId = 1
            };

            await refereeService.CreateAsync(firstRefereeViewModel);
            await refereeService.CreateAsync(secondRefereeViewModel);

            var keyValuePairs = refereeService.GetAllAsKeyValuePairs().ToList();

            Assert.True(keyValuePairs.Count == 2);
        }
    }
}
