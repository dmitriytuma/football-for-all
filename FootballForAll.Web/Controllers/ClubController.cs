using FootballForAll.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            var championship = new ChampionshipViewModel();
            var clubViewModel = new ClubViewModel(championship);
            return View(clubViewModel);
        }
    }
}
