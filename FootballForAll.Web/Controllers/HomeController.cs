using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballForAll.ViewModels.Main;
using FootballForAll.Services.Interfaces;

namespace FootballForAll.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMatchService matchService;

        public HomeController(ILogger<HomeController> logger, IMatchService matchService)
        {
            _logger = logger;
            this.matchService = matchService;
        }

        public IActionResult Index()
        {
            var matches = matchService.GetAllGroupedByChampionships();

            var fixtures = matches.Select(group => new FixturesViewModel
            {
                SeasonId = group.Key.Id,
                ChampionshipName = group.Key.Championship.Name,
                Matches = group
                    .Select(match => new MatchBasicInfoViewModel
                    {
                        MatchId = match.Id,
                        PlayedOn = match.PlayedOn,
                        HomeTeamName = match.HomeTeam.Name,
                        AwayTeamName = match.AwayTeam.Name,
                        HomeTeamGoals = match.HomeTeamGoals,
                        AwayTeamGoals = match.AwayTeamGoals
                    }).ToList()
            });

            return View(fixtures);
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
