using Microsoft.AspNetCore.Mvc;

namespace DunkerFinal.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
