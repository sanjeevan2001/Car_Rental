using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CustomerDashboardController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Customer Dashboard Home
        public IActionResult Index()
        {
            return View();
        }

        // GET: View all available cars
        public async Task<IActionResult> AvailableCars()
        {
            var cars = await _dbContext.Cars
                .Where(c => c.IsAvailable)
                .ToListAsync();

            return View(cars);
        }

        // GET: View customer's bookings
        public async Task<IActionResult> MyBookings(Guid? customerId)
        {
            if (customerId == null)
                return BadRequest("Customer ID is required.");

            var bookings = await _dbContext.Bookings
                .Include(b => b.Car)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Book a car
        public async Task<IActionResult> BookCar(Guid carId)
        {
            var car = await _dbContext.Cars.FindAsync(carId);
            if (car == null || !car.IsAvailable)
            {
                return NotFound("Car is not available.");
            }

            return View(car);
        }

        // POST: Book a car
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookCar(Guid carId, Guid customerId, DateTime pickupDate, DateTime returnDate)
        {
            if (pickupDate >= returnDate)
            {
                ModelState.AddModelError("", "Return date must be after pickup date.");
                return RedirectToAction(nameof(BookCar), new { carId });
            }

            var car = await _dbContext.Cars.FindAsync(carId);
            if (car == null || !car.IsAvailable)
            {
                return NotFound("Car is not available.");
            }

            // Calculate total cost
            var totalDays = (returnDate - pickupDate).Days;
            var totalCost = totalDays * car.PricePerday;

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                CarId = carId,
                CustomerId = customerId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                TotalCost = totalCost
            };

            _dbContext.Bookings.Add(booking);

            // Mark car as unavailable
            car.IsAvailable = false;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MyBookings), new { customerId });
        }
    }
}
