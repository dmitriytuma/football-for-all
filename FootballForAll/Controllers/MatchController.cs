using FootballForAll.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult Index()
        {
            var championship = new ChampionshipViewModel();
            var club = new ClubViewModel(championship);
            var matchViewModel = new MatchViewModel(championship, club);

            return View(matchViewModel);
        }
    }
}
