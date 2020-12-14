using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class SeasonTableController : AdminController
    {
        private readonly ISeasonTableService seasonTableService;
        private readonly ISeasonService seasonService;
        private readonly IClubService clubService;

        public SeasonTableController(
            ISeasonTableService seasonTableService,
            ISeasonService seasonService,
            IClubService clubService)
        {
            this.seasonTableService = seasonTableService;
            this.seasonService = seasonService;
            this.clubService = clubService;
        }

        public IActionResult Index()
        {
            var seasonTables = seasonTableService.GetAll();
            var seasonTableViewModels = seasonTables.Select(s => new SeasonTableViewModel
            {
                Id = s.Id,
                Points = s.Points,
                Won = s.Won,
                Drawn = s.Drawn,
                Lost = s.Lost,
                GoalsFor = s.GoalsFor,
                GoalsAgainst = s.GoalsAgainst,
                SeasonId = s.Season.Id,
                SeasonName = $"{s.Season.Championship.Name} / {s.Season.Name}",
                ClubId = s.Club.Id,
                ClubName = s.Club.Name
            });

            return View(seasonTableViewModels);
        }

        public IActionResult Create()
        {
            var seasonTableViewModel = new SeasonTableViewModel
            {
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(seasonTableViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeasonTableViewModel seasonTableViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                seasonTableViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                seasonTableViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(seasonTableViewModel);
            }
            try
            {
                await seasonTableService.CreateAsync(seasonTableViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                seasonTableViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                seasonTableViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(seasonTableViewModel);
            }

            TempData["SuccessMessage"] = "SeasonTable added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var seasonTable = seasonTableService.Get(id);
            var seasonTableViewModel = new SeasonTableViewModel
            {
                Id = id,
                Points = seasonTable.Points,
                Won = seasonTable.Won,
                Drawn = seasonTable.Drawn,
                Lost = seasonTable.Lost,
                GoalsFor = seasonTable.GoalsFor,
                GoalsAgainst = seasonTable.GoalsAgainst,
                SeasonId = seasonTable.Season.Id,
                SeasonName = $"{seasonTable.Season.Championship.Name} / {seasonTable.Season.Name}",
                ClubId = seasonTable.Club.Id,
                ClubName = seasonTable.Club.Name,
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(seasonTableViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeasonTableViewModel seasonTableViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                seasonTableViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                seasonTableViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(seasonTableViewModel);
            }

            try
            {
                await seasonTableService.UpdateAsync(seasonTableViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                seasonTableViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                seasonTableViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(seasonTableViewModel);
            }

            TempData["SuccessMessage"] = $"SeasonTable  updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var seasonTable = seasonTableService.Get(id);
            var seasonTableViewModel = new SeasonTableViewModel
            {
                Id = id,
                Points = seasonTable.Points,
                Won = seasonTable.Won,
                Drawn = seasonTable.Drawn,
                Lost = seasonTable.Lost,
                GoalsFor = seasonTable.GoalsFor,
                GoalsAgainst = seasonTable.GoalsAgainst,
                SeasonId = seasonTable.Season.Id,
                SeasonName = $"{seasonTable.Season.Championship.Name} / {seasonTable.Season.Name}",
                ClubId = seasonTable.Club.Id,
                ClubName = seasonTable.Club.Name
            };

            ViewData["EntityName"] = "SeasonTable";

            return View("../Shared/_Delete", seasonTableViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeasonTableViewModel seasonTableViewModel)
        {
            try
            {
                await seasonTableService.DeleteAsync(seasonTableViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "SeasonTable";

                return View("../Shared/_Delete", seasonTableViewModel);
            }

            TempData["SuccessMessage"] = "SeasonTable deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
