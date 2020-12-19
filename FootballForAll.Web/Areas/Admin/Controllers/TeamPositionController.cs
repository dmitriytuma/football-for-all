using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class TeamPositionController : AdminController
    {
        private readonly ITeamPositionService teamPositionService;
        private readonly ISeasonService seasonService;
        private readonly IClubService clubService;

        public TeamPositionController(
            ITeamPositionService teamPositionService,
            ISeasonService seasonService,
            IClubService clubService)
        {
            this.teamPositionService = teamPositionService;
            this.seasonService = seasonService;
            this.clubService = clubService;
        }

        public IActionResult Index()
        {
            var teamPositions = teamPositionService.GetAll();
            var teamPositionViewModels = teamPositions.Select(s => new TeamPositionViewModel
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

            return View(teamPositionViewModels);
        }

        public IActionResult Create()
        {
            var teamPositionViewModel = new TeamPositionViewModel
            {
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(teamPositionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamPositionViewModel teamPositionViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                teamPositionViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                teamPositionViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(teamPositionViewModel);
            }
            try
            {
                await teamPositionService.CreateAsync(teamPositionViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                teamPositionViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                teamPositionViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(teamPositionViewModel);
            }

            TempData["SuccessMessage"] = "TeamPosition added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var teamPosition = teamPositionService.Get(id);
            var teamPositionViewModel = new TeamPositionViewModel
            {
                Id = id,
                Points = teamPosition.Points,
                Won = teamPosition.Won,
                Drawn = teamPosition.Drawn,
                Lost = teamPosition.Lost,
                GoalsFor = teamPosition.GoalsFor,
                GoalsAgainst = teamPosition.GoalsAgainst,
                SeasonId = teamPosition.Season.Id,
                SeasonName = $"{teamPosition.Season.Championship.Name} / {teamPosition.Season.Name}",
                ClubId = teamPosition.Club.Id,
                ClubName = teamPosition.Club.Name,
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(teamPositionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamPositionViewModel teamPositionViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                teamPositionViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                teamPositionViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(teamPositionViewModel);
            }

            try
            {
                await teamPositionService.UpdateAsync(teamPositionViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                teamPositionViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                teamPositionViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(teamPositionViewModel);
            }

            TempData["SuccessMessage"] = $"TeamPosition  updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var teamPosition = teamPositionService.Get(id);
            var teamPositionViewModel = new TeamPositionViewModel
            {
                Id = id,
                Points = teamPosition.Points,
                Won = teamPosition.Won,
                Drawn = teamPosition.Drawn,
                Lost = teamPosition.Lost,
                GoalsFor = teamPosition.GoalsFor,
                GoalsAgainst = teamPosition.GoalsAgainst,
                SeasonId = teamPosition.Season.Id,
                SeasonName = $"{teamPosition.Season.Championship.Name} / {teamPosition.Season.Name}",
                ClubId = teamPosition.Club.Id,
                ClubName = teamPosition.Club.Name
            };

            ViewData["EntityName"] = "TeamPosition";

            return View("../Shared/_Delete", teamPositionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TeamPositionViewModel teamPositionViewModel)
        {
            try
            {
                await teamPositionService.DeleteAsync(teamPositionViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "TeamPosition";

                return View("../Shared/_Delete", teamPositionViewModel);
            }

            TempData["SuccessMessage"] = "TeamPosition deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
