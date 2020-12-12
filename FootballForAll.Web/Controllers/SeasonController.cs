using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballForAll.Data;
using FootballForAll.Web.Models;
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

            var seasonViewModel = new SeasonViewModel
            {
                ChampionshipName = season.Championship.Name,
                Name = season.Name,
                Country = season.Championship.Country.Name
            };

            return View(seasonViewModel);
        }
    }
}
