using FootballForAll.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Controllers
{
    public class ChampionshipController : Controller
    {
        public IActionResult Index()
        {
            var championshipViewModel = new ChampionshipViewModel();
            return View(championshipViewModel);
        }
    }
}
