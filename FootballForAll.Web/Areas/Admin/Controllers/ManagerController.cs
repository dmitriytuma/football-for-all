using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin.People;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class ManagerController : AdminController
    {
        private readonly IManagerService managerService;
        private readonly ICountryService countryService;
        private readonly IClubService clubService;

        public ManagerController(
            IManagerService managerService,
            ICountryService countryService,
            IClubService clubService)
        {
            this.managerService = managerService;
            this.countryService = countryService;
            this.clubService = clubService;
        }

        public IActionResult Index()
        {
            var managers = managerService.GetAll();
            var managerViewModels = managers.Select(c => new ManagerViewModel
            {
                Id = c.Id,
                Name = c.Name,
                BirthDate = c.BirthDate,
                CountryId = c.Country.Id,
                CountryName = c.Country.Name,
                ClubId = c.Club.Id,
                ClubName = c.Club.Name
            });

            return View(managerViewModels);
        }

        public IActionResult Create()
        {
            var managerViewModel = new ManagerViewModel
            {
                BirthDate = DateTime.Now,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(managerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ManagerViewModel managerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                managerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                managerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(managerViewModel);
            }

            try
            {
                await managerService.CreateAsync(managerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                managerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                managerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(managerViewModel);
            }

            TempData["SuccessMessage"] = "Manager added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var manager = managerService.Get(id);
            var managerViewModel = new ManagerViewModel
            {
                Id = id,
                Name = manager.Name,
                BirthDate = manager.BirthDate,
                CountryId = manager.Country.Id,
                CountryName = manager.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                ClubId = manager.Club.Id,
                ClubName = manager.Club.Name,
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(managerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ManagerViewModel managerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                managerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                managerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(managerViewModel);
            }

            try
            {
                await managerService.UpdateAsync(managerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                managerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                managerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(managerViewModel);
            }

            TempData["SuccessMessage"] = $"Manager {managerViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var manager = managerService.Get(id);
            var managerViewModel = new ManagerViewModel
            {
                Id = id,
                Name = manager.Name,
                BirthDate = manager.BirthDate,
                CountryId = manager.Country.Id,
                CountryName = manager.Country.Name,
                ClubId = manager.Club.Id,
                ClubName = manager.Club.Name
            };

            ViewData["EntityName"] = "Manager";

            return View("../Shared/_Delete", managerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ManagerViewModel managerViewModel)
        {
            try
            {
                await managerService.DeleteAsync(managerViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Manager";

                return View("../Shared/_Delete", managerViewModel);
            }

            TempData["SuccessMessage"] = "Manager deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
