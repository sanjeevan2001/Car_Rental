using CarRental.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class GuestController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public GuestController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GuestView()
        {
            var cars = _appDbContext.Cars.ToList();
            return View(cars);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
       
        public IActionResult news1()
        {
            return View();
        }
    }
}
