using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.ViewModels.Admin;

namespace FootballForAll.Services.Interfaces
{
    public interface ICountryService
    {
        Country Get(int id);

        IEnumerable<Country> GetAll();

        Task CreateAsync(CountryViewModel countryViewModel);

        Task UpdateAsync(CountryViewModel countryViewModel);

        Task DeleteAsync(int id);

    }
}
