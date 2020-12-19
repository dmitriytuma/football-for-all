using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class ClubController : AdminController
    {
        private readonly IClubService clubService;
        private readonly ICountryService countryService;
        private readonly IStadiumService stadiumService;

        public ClubController(
            IClubService clubService,
            ICountryService countryService,
            IStadiumService stadiumService)
        {
            this.clubService = clubService;
            this.countryService = countryService;
            this.stadiumService = stadiumService;
        }

        public IActionResult Index()
        {
            var clubs = clubService.GetAll();
            var clubViewModels = clubs.Select(c => new ClubViewModel
            {
                Id = c.Id,
                Name = c.Name,
                FoundedOn = c.FoundedOn,
                CountryId = c.Country.Id,
                CountryName = c.Country.Name,
                HomeStadiumId = c.HomeStadium.Id,
                HomeStadiumName = c.HomeStadium.Name
            });

            return View(clubViewModels);
        }

        public IActionResult Create()
        {
            var clubViewModel = new ClubViewModel
            {
                FoundedOn = DateTime.Now,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                StadiumsItems = stadiumService.GetAllAsKeyValuePairs()
            };

            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                clubViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                clubViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();

                return View(clubViewModel);
            }

            try
            {
                await clubService.CreateAsync(clubViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                clubViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                clubViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();

                return View(clubViewModel);
            }

            TempData["SuccessMessage"] = "Club added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var club = clubService.Get(id);
            var clubViewModel = new ClubViewModel
            {
                Id = id,
                Name = club.Name,
                FoundedOn = club.FoundedOn,
                CountryId = club.Country.Id,
                CountryName = club.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                HomeStadiumId = club.HomeStadium.Id,
                HomeStadiumName = club.HomeStadium.Name,
                StadiumsItems = stadiumService.GetAllAsKeyValuePairs()
            };

            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                clubViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                clubViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();

                return View(clubViewModel);
            }

            try
            {
                await clubService.UpdateAsync(clubViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                clubViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                clubViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();

                return View(clubViewModel);
            }

            TempData["SuccessMessage"] = $"Club {clubViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var club = clubService.Get(id);
            var clubViewModel = new ClubViewModel
            {
                Id = id,
                Name = club.Name,
                FoundedOn = club.FoundedOn,
                CountryId = club.Country.Id,
                CountryName = club.Country.Name,
                HomeStadiumId = club.HomeStadium.Id,
                HomeStadiumName = club.HomeStadium.Name
            };

            ViewData["EntityName"] = "Club";

            return View("../Shared/_Delete", clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ClubViewModel clubViewModel)
        {
            try
            {
                await clubService.DeleteAsync(clubViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Club";

                return View("../Shared/_Delete", clubViewModel);
            }

            TempData["SuccessMessage"] = "Club deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
