using Microsoft.AspNetCore.Mvc;

namespace CarRental.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult AddCustomer()
        //{
        //    return View();
        //}

        public IActionResult AdminDashBoard()
        {
            return View();
        }
    }
}
