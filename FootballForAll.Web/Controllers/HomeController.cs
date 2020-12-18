using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballForAll.ViewModels.Main;
using FootballForAll.Services.Interfaces;
using System;

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
            var fixtures = matchService.GetAllGroupedByDate(DateTime.Today);

            return View(fixtures);
        }

        [HttpPost]
        public IActionResult GetAllFixturesByDate(DateTime date)
        {
            var fixtures = matchService.GetAllGroupedByDate(date);

            return new JsonResult(fixtures);
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
