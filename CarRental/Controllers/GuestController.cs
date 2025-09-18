using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GuestView()
        {
            return View();
        }

       
    }
}
