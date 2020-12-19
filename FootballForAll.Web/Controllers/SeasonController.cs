using System;
using System.Linq;
using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Main;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ITeamPositionService teamPositionService;
        private readonly ISeasonService seasonService;

        public SeasonController(ITeamPositionService teamPositionService, ISeasonService seasonService)
        {
            this.teamPositionService = teamPositionService;
            this.seasonService = seasonService;
        }

        public IActionResult Index(int id)
        {
            var teamPosition = teamPositionService.GetChampionshipSeasonPositions(id).ToList();

            if (teamPosition.Any())
            {
                var seasonDetais = new SeasonDetailsViewModel
                {
                    ChampionshipName = teamPosition[0].Season.Championship.Name,
                    SeasonName = teamPosition[0].Season.Name,
                    Country = teamPosition[0].Season.Championship.Country.Name,
                    Table = teamPosition.Select(s => new TeamPositionViewModel
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
