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
        private readonly ISeasonService seasonService;

        public SeasonController(ISeasonTableService seasonTableService, ISeasonService seasonService)
        {
            this.seasonTableService = seasonTableService;
            this.seasonService = seasonService;
        }

        public IActionResult Index(int id)
        {
            var seasonTable = seasonTableService.GetChampionshipSeasonPositions(id).ToList();

            if (seasonTable.Any())
            {
                var seasonDetais = new SeasonDetailsViewModel
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
                };

                return View(seasonDetais);
            }
            else
            {
                var season = seasonService.Get(id);
                var seasonDetais = new SeasonDetailsViewModel()
                {
                    ChampionshipName = season.Championship.Name,
                    SeasonName = season.Name
                };

                return View(seasonDetais);
            }
        }
    }
}
