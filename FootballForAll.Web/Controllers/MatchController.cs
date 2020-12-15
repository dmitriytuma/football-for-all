using FootballForAll.Services.Interfaces;
using FootballForAll.ViewModels.Main;
using Microsoft.AspNetCore.Mvc;

namespace FootballForAll.Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        public IActionResult Index(int id)
        {
            var match = matchService.Get(id);

            var matchViewModel = new MatchDetailsViewModel
            {
                SeasonId = match.Season.Id,
                ChampionshipName = match.Season.Championship.Name,
                PlayedOn = match.PlayedOn,
                HomeTeamName = match.HomeTeam.Name,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                AwayTeamName = match.AwayTeam.Name,
                StadiumName = match.Stadium.Name,
                Attendance = match.Attendance,
                MainRefereeName = match.Referee.Name
            };

            return View(matchViewModel);
        }
    }
}
