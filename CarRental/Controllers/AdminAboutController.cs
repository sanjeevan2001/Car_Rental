using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminAboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
