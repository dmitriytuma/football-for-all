using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class StadiumController : AdminController
    {
        private readonly IStadiumService stadiumService;
        private readonly ICountryService countryService;

        public StadiumController(IStadiumService stadiumService, ICountryService countryService)
        {
            this.stadiumService = stadiumService;
            this.countryService = countryService;
        }

        public IActionResult Index()
        {
            var stadiums = stadiumService.GetAll();
            var stadiumViewModels = stadiums.Select(s => new StadiumViewModel
            {
                Id = s.Id,
                Name = s.Name,
                FoundedOn = s.FoundedOn,
                Capacity = s.Capacity,
                CountryId = s.Country.Id,
                CountryName = s.Country.Name
            });

            return View(stadiumViewModels);
        }

        public IActionResult Create()
        {
            var stadiumViewModel = new StadiumViewModel
            {
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(stadiumViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StadiumViewModel stadiumViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Data is not valid.");
            }
            try
            {
                await stadiumService.CreateAsync(stadiumViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                return View(stadiumViewModel);
            }

            TempData["SuccessMessage"] = "Stadium added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var stadium = stadiumService.Get(id);
            var stadiumViewModel = new StadiumViewModel
            {
                Id = id,
                Name = stadium.Name,
                FoundedOn = stadium.FoundedOn,
                Capacity = stadium.Capacity,
                CountryId = stadium.Country.Id,
                CountryName = stadium.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(stadiumViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StadiumViewModel stadiumViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Data is not valid.");
            }

            try
            {
                await stadiumService.UpdateAsync(stadiumViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                return View(stadiumViewModel);
            }

            TempData["SuccessMessage"] = $"Stadium {stadiumViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var stadium = stadiumService.Get(id);
            var stadiumViewModel = new StadiumViewModel
            {
                Id = id,
                Name = stadium.Name,
                FoundedOn = stadium.FoundedOn,
                Capacity = stadium.Capacity,
                CountryId = stadium.Country.Id,
                CountryName = stadium.Country.Name,
            };

            ViewData["EntityName"] = "Stadium";

            return View("../Shared/_Delete", stadiumViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(StadiumViewModel stadiumViewModel)
        {
            try
            {
                await stadiumService.DeleteAsync(stadiumViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Stadium";

                return View("../Shared/_Delete", stadiumViewModel);
            }

            TempData["SuccessMessage"] = "Stadium deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
