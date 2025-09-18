using Microsoft.AspNetCore.Mvc;

namespace CarRental.Areas.Customer.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
