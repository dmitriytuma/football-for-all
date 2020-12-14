using System.Linq;
using FootballForAll.Data;
using FootballForAll.ViewModels.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Web.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SeasonController(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var season = dbContext.Seasons
                .Include(s => s.Championship)
                .ThenInclude(c => c.Country)
                .FirstOrDefault();

            var seasonViewModel = new SeasonStatisticsViewModel
            {
                ChampionshipName = season.Championship.Name,
                Name = season.Name,
                Country = season.Championship.Country.Name
            };

            return View(seasonViewModel);
        }
    }
}
