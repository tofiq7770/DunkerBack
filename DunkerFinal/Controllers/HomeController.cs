using Microsoft.AspNetCore.Mvc;

namespace DunkerFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}