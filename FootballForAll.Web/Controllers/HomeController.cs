using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballForAll.ViewModels.Main;
using FootballForAll.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            dbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var firstSeason = dbContext.Seasons
                .Include(s => s.Championship)
                .ThenInclude(c => c.Country)
                .FirstOrDefault();
            var championship = firstSeason.Championship;
            var championshipCountry = firstSeason.Championship.Country;
            var matches = dbContext.Matches
                .Include(m => m.Season)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.Season == firstSeason)
                .ToList();

            var seasonViewModel = new SeasonStatisticsViewModel
            {
                Id = firstSeason.Id,
                Name = firstSeason.Name,
                ChampionshipName = championship.Name,
                Country = championshipCountry.Name,
                Matches = new List<MatchViewModel>
                {
                    new MatchViewModel
                    {
                        HomeTeamName = matches[0].HomeTeam.Name,
                        AwayTeamName = matches[0].AwayTeam.Name,
                        HomeTeamGoals = matches[0].HomeTeamGoals,
                        AwayTeamGoals = matches[0].AwayTeamGoals
                    },
                    new MatchViewModel
                    {
                        HomeTeamName = matches[1].HomeTeam.Name,
                        AwayTeamName = matches[1].AwayTeam.Name,
                        HomeTeamGoals = matches[1].HomeTeamGoals,
                        AwayTeamGoals = matches[1].AwayTeamGoals
                    },
                    new MatchViewModel
                    {
                        HomeTeamName = matches[2].HomeTeam.Name,
                        AwayTeamName = matches[2].AwayTeam.Name,
                        HomeTeamGoals = matches[2].HomeTeamGoals,
                        AwayTeamGoals = matches[2].AwayTeamGoals
                    },
                }
            };

            var seasons = new List<SeasonStatisticsViewModel> { seasonViewModel };

            return View(seasons);
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
