using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
