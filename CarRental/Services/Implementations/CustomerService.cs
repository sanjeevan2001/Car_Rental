using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services.Implementations
{
    public class CustomerService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
