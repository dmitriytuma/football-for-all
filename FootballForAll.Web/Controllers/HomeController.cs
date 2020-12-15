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

            var seasonViewModel = new SeasonDetailsViewModel
            {
                Id = firstSeason.Id,
                SeasonName = firstSeason.Name,
                ChampionshipName = championship.Name,
                Country = championshipCountry.Name,
                Matches = matches.Select(m => new MatchDetailsViewModel
                {
                    PlayedOn = m.PlayedOn,
                    HomeTeamName = m.HomeTeam.Name,
                    AwayTeamName = m.AwayTeam.Name,
                    HomeTeamGoals = m.HomeTeamGoals,
                    AwayTeamGoals = m.AwayTeamGoals
                }).ToList()
            };

            var seasons = new List<SeasonDetailsViewModel> { seasonViewModel };

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
