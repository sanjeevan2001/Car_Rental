using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminBookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
