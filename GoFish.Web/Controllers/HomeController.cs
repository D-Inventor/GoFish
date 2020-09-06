using System.Diagnostics;
using System.Threading.Tasks;

using GoFish.Web.Hubs;
using GoFish.Web.Models;
using GoFish.Web.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoFish.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameAccessor _gameProvider;
        private readonly IHubContext<GameHub> _gameHub;

        public HomeController(IGameAccessor gameProvider, IHubContext<GameHub> gameHub)
        {
            _gameProvider = gameProvider;
            _gameHub = gameHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Reset()
        {
            _gameProvider.Game = null;
            await _gameHub.Clients.All.SendAsync("ReceiveGameChange", "Reset", null);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
