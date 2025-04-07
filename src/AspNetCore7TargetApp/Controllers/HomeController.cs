using Microsoft.AspNetCore.Mvc;
using MultiTargetLibrary;
using MultiTargetWebApp.Models;
using System.Diagnostics;

namespace MultiTargetWebApp.Controllers
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
            var aPerson = new Person("John", "Doe");
            var jsonPerson = PersonSerializer.Serialize(aPerson);
            return View("Index", jsonPerson);
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
