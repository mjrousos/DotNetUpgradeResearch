using AspNetCore22App.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore22App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new UserInformation
            {
                Agent = HelperService.GetUserAgent(HttpContext)
            });
        }
    }
}
