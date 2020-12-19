using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using FootballForAll.ViewModels.Admin.People;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class RefereeController : AdminController
    {
        private readonly IRefereeService refereeService;
        private readonly ICountryService countryService;

        public RefereeController(IRefereeService refereeService, ICountryService countryService)
        {
            this.refereeService = refereeService;
            this.countryService = countryService;
        }

        public IActionResult Index()
        {
            var referees = refereeService.GetAll();
            var refereeViewModels = referees.Select(r => new RefereeViewModel
            {
                Id = r.Id,
                Name = r.Name,
                BirthDate = r.BirthDate,
                CountryId = r.Country.Id,
                CountryName = r.Country.Name
            });

            return View(refereeViewModels);
        }

        public IActionResult Create()
        {
            var refereeViewModel = new RefereeViewModel
            {
                BirthDate = DateTime.Now,
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(refereeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RefereeViewModel refereeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                refereeViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(refereeViewModel);
            }
            try
            {
                await refereeService.CreateAsync(refereeViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                refereeViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(refereeViewModel);
            }

            TempData["SuccessMessage"] = "Referee added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var referee = refereeService.Get(id);
            var refereeViewModel = new RefereeViewModel
            {
                Id = id,
                Name = referee.Name,
                BirthDate = referee.BirthDate,
                CountryId = referee.Country.Id,
                CountryName = referee.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs()
            };

            return View(refereeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RefereeViewModel refereeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                refereeViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(refereeViewModel);
            }

            try
            {
                await refereeService.UpdateAsync(refereeViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                refereeViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();

                return View(refereeViewModel);
            }

            TempData["SuccessMessage"] = $"Referee {refereeViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var referee = refereeService.Get(id);
            var refereeViewModel = new RefereeViewModel
            {
                Id = id,
                Name = referee.Name,
                BirthDate = referee.BirthDate,
                CountryId = referee.Country.Id,
                CountryName = referee.Country.Name,
            };

            ViewData["EntityName"] = "Referee";

            return View("../Shared/_Delete", refereeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RefereeViewModel refereeViewModel)
        {
            try
            {
                await refereeService.DeleteAsync(refereeViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Referee";

                return View("../Shared/_Delete", refereeViewModel);
            }

            TempData["SuccessMessage"] = "Referee deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
