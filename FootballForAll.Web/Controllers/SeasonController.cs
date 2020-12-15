using System;
using System.Linq;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Main;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonTableService seasonTableService;

        public SeasonController(ISeasonTableService seasonTableService)
        {
            this.seasonTableService = seasonTableService;
        }

        public IActionResult Index(int id)
        {
            var seasonTable = seasonTableService.GetChampionshipSeasonPositions(id).ToList();

            var seasonDetais = seasonTable.Any()
                ? new SeasonDetailsViewModel
                {
                    ChampionshipName = seasonTable[0].Season.Championship.Name,
                    SeasonName = seasonTable[0].Season.Name,
                    Country = seasonTable[0].Season.Championship.Country.Name,
                    Table = seasonTable.Select(s => new TeamPositionViewModel
                    {
                        TeamName = s.Club.Name,
                        Points = s.Points,
                        Won = s.Won,
                        Drawn = s.Drawn,
                        Lost = s.Lost,
                        GoalsFor = s.GoalsFor,
                        GoalsAgainst = s.GoalsAgainst
                    }).ToList()
                }
                : new SeasonDetailsViewModel();

            return View(seasonDetais);
        }
    }
}
