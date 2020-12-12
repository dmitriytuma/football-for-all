using FootballForAll.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Controllers
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
