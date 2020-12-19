using System;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Admin.People;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Areas.Admin.Controllers
{
    public class PlayerController : AdminController
    {
        private readonly IPlayerService playerService;
        private readonly ICountryService countryService;
        private readonly IClubService clubService;

        public PlayerController(
            IPlayerService playerService,
            ICountryService countryService,
            IClubService clubService)
        {
            this.playerService = playerService;
            this.countryService = countryService;
            this.clubService = clubService;
        }

        public IActionResult Index()
        {
            var players = playerService.GetAll();
            var playerViewModels = players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate,
                Number = p.Number,
                Position = p.Position,
                Goals = p.Goals,
                YellowCards = p.YellowCards,
                RedCards = p.RedCards,
                CountryId = p.Country.Id,
                CountryName = p.Country.Name,
                ClubId = p.Club.Id,
                ClubName = p.Club.Name
            });

            return View(playerViewModels);
        }

        public IActionResult Create()
        {
            var playerViewModel = new PlayerViewModel
            {
                BirthDate = DateTime.Now,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(playerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerViewModel playerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                playerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                playerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(playerViewModel);
            }

            try
            {
                await playerService.CreateAsync(playerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                playerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                playerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(playerViewModel);
            }

            TempData["SuccessMessage"] = "Player added successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var player = playerService.Get(id);
            var playerViewModel = new PlayerViewModel
            {
                Id = id,
                Name = player.Name,
                BirthDate = player.BirthDate,
                Number = player.Number,
                Position = player.Position,
                Goals = player.Goals,
                YellowCards = player.YellowCards,
                RedCards = player.RedCards,
                CountryId = player.Country.Id,
                CountryName = player.Country.Name,
                CountriesItems = countryService.GetAllAsKeyValuePairs(),
                ClubId = player.Club.Id,
                ClubName = player.Club.Name,
                ClubsItems = clubService.GetAllAsKeyValuePairs()
            };

            return View(playerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlayerViewModel playerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");

                playerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                playerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(playerViewModel);
            }

            try
            {
                await playerService.UpdateAsync(playerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                playerViewModel.CountriesItems = countryService.GetAllAsKeyValuePairs();
                playerViewModel.ClubsItems = clubService.GetAllAsKeyValuePairs();

                return View(playerViewModel);
            }

            TempData["SuccessMessage"] = $"Player {playerViewModel.Name} updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var player = playerService.Get(id);
            var playerViewModel = new PlayerViewModel
            {
                Id = id,
                Name = player.Name,
                BirthDate = player.BirthDate,
                Number = player.Number,
                Position = player.Position,
                Goals = player.Goals,
                YellowCards = player.YellowCards,
                RedCards = player.RedCards,
                CountryId = player.Country.Id,
                CountryName = player.Country.Name,
                ClubId = player.Club.Id,
                ClubName = player.Club.Name
            };

            ViewData["EntityName"] = "Player";

            return View("../Shared/_Delete", playerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PlayerViewModel playerViewModel)
        {
            try
            {
                await playerService.DeleteAsync(playerViewModel.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                ViewData["EntityName"] = "Player";

                return View("../Shared/_Delete", playerViewModel);
            }

            TempData["SuccessMessage"] = "Player deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
