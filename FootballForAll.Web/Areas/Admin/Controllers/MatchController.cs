using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class MatchController : AdminController
    {
        private readonly IMatchService matchService;
        private readonly IClubService clubService;
        private readonly ISeasonService seasonService;
        private readonly IStadiumService stadiumService;
        private readonly IRefereeService refereeService;

        public MatchController(
            IMatchService matchService,
            IClubService clubService,
            ISeasonService seasonService,
            IStadiumService stadiumService,
            IRefereeService refereeService)
        {
            this.matchService = matchService;
            this.clubService = clubService;
            this.seasonService = seasonService;
            this.stadiumService = stadiumService;
            this.refereeService = refereeService;
        }

        public IActionResult Index()
        {
            var matchs = matchService.GetAll();
            var matchViewModels = matchs.Select(m => new MatchViewModel
            {
                Id = m.Id,
                HomeTeamGoals = m.HomeTeamGoals,
                AwayTeamGoals = m.AwayTeamGoals,
                Attendance = m.Attendance,
                PlayedOn = m.PlayedOn,
                HomeTeamId = m.HomeTeam.Id,
                HomeTeamName = m.HomeTeam.Name,
                AwayTeamId = m.AwayTeam.Id,
                AwayTeamName = m.AwayTeam.Name,
                SeasonId = m.Season.Id,
                SeasonName = $"{m.Season.Championship.Name} / {m.Season.Name}",
                StadiumId = m.Stadium.Id,
                StadiumName = m.Stadium.Name,
                RefereeId = m.Referee.Id,
                RefereeName = m.Referee.Name
            });

            return View(matchViewModels);
        }

        public IActionResult Create()
        {
            var matchViewModel = new MatchViewModel
            {
                PlayedOn = DateTime.Now,
                ClubsItems = clubService.GetAllAsKeyValuePairs(),
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                StadiumsItems = stadiumService.GetAllAsKeyValuePairs(),
                RefereesItems = refereeService.GetAllAsKeyValuePairs()
            };

            return View(matchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MatchViewModel matchViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                matchViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();
                matchViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                matchViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();
                matchViewModel.RefereesItems = refereeService.GetAllAsKeyValuePairs();

                return View(matchViewModel);
            }
            try
            {
                await matchService.CreateAsync(matchViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                matchViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();
                matchViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                matchViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();
                matchViewModel.RefereesItems = refereeService.GetAllAsKeyValuePairs();

                return View(matchViewModel);
            }

            TempData["SuccessMessage"] = "Match added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var match = matchService.Get(id);
            var matchViewModel = new MatchViewModel
            {
                Id = match.Id,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                Attendance = match.Attendance,
                PlayedOn = match.PlayedOn,
                HomeTeamId = match.HomeTeam.Id,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamId = match.AwayTeam.Id,
                AwayTeamName = match.AwayTeam.Name,
                SeasonId = match.Season.Id,
                SeasonName = $"{match.Season.Championship.Name} / {match.Season.Name}",
                StadiumId = match.Stadium.Id,
                StadiumName = match.Stadium.Name,
                RefereeId = match.Referee.Id,
                RefereeName = match.Referee.Name,
                ClubsItems = clubService.GetAllAsKeyValuePairs(),
                SeasonsItems = seasonService.GetAllAsKeyValuePairs(),
                StadiumsItems = stadiumService.GetAllAsKeyValuePairs(),
                RefereesItems = refereeService.GetAllAsKeyValuePairs()
            };

            return View(matchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MatchViewModel matchViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                matchViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();
                matchViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                matchViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();
                matchViewModel.RefereesItems = refereeService.GetAllAsKeyValuePairs();

                return View(matchViewModel);
            }

            try
            {
                await matchService.UpdateAsync(matchViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                matchViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();
                matchViewModel.SeasonsItems = seasonService.GetAllAsKeyValuePairs();
                matchViewModel.StadiumsItems = stadiumService.GetAllAsKeyValuePairs();
                matchViewModel.RefereesItems = refereeService.GetAllAsKeyValuePairs();

                return View(matchViewModel);
            }

            TempData["SuccessMessage"] = $"Match  updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var match = matchService.Get(id);
            var matchViewModel = new MatchViewModel
            {
                Id = match.Id,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                Attendance = match.Attendance,
                PlayedOn = match.PlayedOn,
                HomeTeamId = match.HomeTeam.Id,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamId = match.AwayTeam.Id,
                AwayTeamName = match.AwayTeam.Name,
                SeasonId = match.Season.Id,
                SeasonName = $"{match.Season.Championship.Name} / {match.Season.Name}",
                StadiumId = match.Stadium.Id,
                StadiumName = match.Stadium.Name,
                RefereeId = match.Referee.Id,
                RefereeName = match.Referee.Name
            };

            ViewData["EntityName"] = "Match";

            return View("../Shared/_Delete", matchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MatchViewModel matchViewModel)
        {
            try
            {
                await matchService.DeleteAsync(matchViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Match";

                return View("../Shared/_Delete", matchViewModel);
            }

            TempData["SuccessMessage"] = "Match deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
