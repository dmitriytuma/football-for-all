using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class ChampionshipController : AdminController
    {
        private readonly IChampionshipService championshipService;
        private readonly ICountryService countryService;

        public ChampionshipController(IChampionshipService championshipService, ICountryService countryService)
        {
            this.championshipService = championshipService;
            this.countryService = countryService;
        }

        public IActionResult Index()
        {
            var championships = championshipService.GetAll();
            var championshipViewModels = championships.Select(c => new ChampionshipViewModel
            {
                Id = c.Id,
                Name = c.Name,
                FoundedOn = c.FoundedOn,
                Description = c.Description,
                CountryId = c.Country.Id,
                CountryName = c.Country.Name
            });

            return View(championshipViewModels);
        }

        public IActionResult Create()
        {
            var championshipViewModel = new ChampionshipViewModel
            {
                FoundedOn = DateTime.Now,
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(championshipViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChampionshipViewModel championshipViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                championshipViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(championshipViewModel);
            }
            try
            {
                await championshipService.CreateAsync(championshipViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                championshipViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(championshipViewModel);
            }

            TempData["SuccessMessage"] = "Championship added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var championship = championshipService.Get(id);
            var championshipViewModel = new ChampionshipViewModel
            {
                Id = id,
                Name = championship.Name,
                FoundedOn = championship.FoundedOn,
                Description = championship.Description,
                CountryId = championship.Country.Id,
                CountryName = championship.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(championshipViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChampionshipViewModel championshipViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                championshipViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(championshipViewModel);
            }

            try
            {
                await championshipService.UpdateAsync(championshipViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                championshipViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(championshipViewModel);
            }

            TempData["SuccessMessage"] = $"Championship {championshipViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var championship = championshipService.Get(id);
            var championshipViewModel = new ChampionshipViewModel
            {
                Id = id,
                Name = championship.Name,
                FoundedOn = championship.FoundedOn,
                Description = championship.Description,
                CountryId = championship.Country.Id,
                CountryName = championship.Country.Name,
            };

            ViewData["EntityName"] = "Championship";

            return View("../Shared/_Delete", championshipViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ChampionshipViewModel championshipViewModel)
        {
            try
            {
                await championshipService.DeleteAsync(championshipViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Championship";

                return View("../Shared/_Delete", championshipViewModel);
            }

            TempData["SuccessMessage"] = "Championship deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
