using FileStorageSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileStorageSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.userId = HttpContext.Session.GetString("userId");
            ViewBag.fullName = HttpContext.Session.GetString("fullName");
            return View();
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

        public IActionResult logOut()
        {
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("fullName");
            return RedirectToAction("index", "Login");
        }
    }
}