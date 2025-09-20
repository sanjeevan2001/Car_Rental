using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
