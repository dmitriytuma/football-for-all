using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class SeasonController : AdminController
    {
        private readonly ISeasonService seasonService;
        private readonly IChampionshipService championshipService;

        public SeasonController(ISeasonService seasonService, IChampionshipService championshipService)
        {
            this.seasonService = seasonService;
            this.championshipService = championshipService;
        }

        public IActionResult Index()
        {
            var seasons = seasonService.GetAll();
            var seasonViewModels = seasons.Select(c => new SeasonViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ChampionshipId = c.Championship.Id,
                ChampionshipName = c.Championship.Name
            });

            return View(seasonViewModels);
        }

        public IActionResult Create()
        {
            var seasonViewModel = new SeasonViewModel
            {
                ChampionshipsItems = championshipService.GetAllAsKeyValuePairs()
            };

            return View(seasonViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeasonViewModel seasonViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                seasonViewModel.ChampionshipsItems = championshipService.GetAllAsKeyValuePairs();

                return View(seasonViewModel);
            }
            try
            {
                await seasonService.CreateAsync(seasonViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                seasonViewModel.ChampionshipsItems = championshipService.GetAllAsKeyValuePairs();

                return View(seasonViewModel);
            }

            TempData["SuccessMessage"] = "Season added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var season = seasonService.Get(id);
            var seasonViewModel = new SeasonViewModel
            {
                Id = id,
                Name = season.Name,
                Description = season.Description,
                ChampionshipId = season.Championship.Id,
                ChampionshipName = season.Championship.Name,
                ChampionshipsItems = championshipService.GetAllAsKeyValuePairs()
            };

            return View(seasonViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeasonViewModel seasonViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                seasonViewModel.ChampionshipsItems = championshipService.GetAllAsKeyValuePairs();

                return View(seasonViewModel);
            }

            try
            {
                await seasonService.UpdateAsync(seasonViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                seasonViewModel.ChampionshipsItems = championshipService.GetAllAsKeyValuePairs();

                return View(seasonViewModel);
            }

            TempData["SuccessMessage"] = $"Season {seasonViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var season = seasonService.Get(id);
            var seasonViewModel = new SeasonViewModel
            {
                Id = id,
                Name = season.Name,
                Description = season.Description,
                ChampionshipId = season.Championship.Id,
                ChampionshipName = season.Championship.Name,
            };

            ViewData["EntityName"] = "Season";

            return View("../Shared/_Delete", seasonViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeasonViewModel seasonViewModel)
        {
            try
            {
                await seasonService.DeleteAsync(seasonViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Season";

                return View("../Shared/_Delete", seasonViewModel);
            }

            TempData["SuccessMessage"] = "Season deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
