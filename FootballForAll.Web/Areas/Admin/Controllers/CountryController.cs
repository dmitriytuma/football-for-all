using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class CountryController : AdminController
    {
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        public IActionResult Index()
        {
            var countries = countryService.GetAll();
            var countryViewModels = countries.Select(c => new CountryViewModel
            {
                Name = c.Name,
                Code = c.Code
            });

            return View(countryViewModels);
        }

        public IActionResult Create()
        {
            var countryViewModel = new CountryViewModel();
            return View(countryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryViewModel countryViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Data is not valid.");
            }

            await countryService.CreateAsync(countryViewModel);

            TempData["SuccessMessage"] = "Country added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var country = countryService.Get(id);
            var countryViewModel = new CountryViewModel
            {
                Name = country.Name,
                Code = country.Code
            };

            return View(countryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CountryViewModel countryViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Data is not valid.");
            }

            await countryService.UpdateAsync(id, countryViewModel);

            TempData["SuccessMessage"] = $"Country {countryViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Delete(CountryViewModel countryModel)
        {
            throw new NotImplementedException();
        }
    }
}
