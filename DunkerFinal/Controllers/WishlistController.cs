using Microsoft.AspNetCore.Mvc;

namespace DunkerFinal.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
