using CarRental.Data;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           

            return View();
        }

        public IActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalCars = _context.Cars.Count(),
                AvailableCars = _context.Cars.Count(c => c.IsAvailable),
                TotalBookings = _context.Bookings.Count(),
                TotalCustomers = _context.Customers.Count()
            };

            return View(model);
        }
    }
}
