using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrintUMKServer.Models;
using Microsoft.AspNetCore.Identity;


namespace PrintUMKServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //    return View();

            //var user = await _userManager.GetUserAsync(User);

            //return View(user);
            return RedirectToAction("Upload", "Print");
        }

        public IActionResult Privacy()
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
