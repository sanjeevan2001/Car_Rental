using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly TemData _temData;

        public CustomerDashboardController(AppDbContext dbContext, TemData temData)
        {
            _dbContext = dbContext;
            _temData = temData;
        }

        // GET: Customer Dashboard Home
        public IActionResult Index()
        {
            // Optionally pass customer name to view
            ViewBag.CustomerName = _temData.CustomerName;
            return View();
        }

        // GET: View all available cars
        public async Task<IActionResult> AvailableCars()
        {
            var cars = await _dbContext.Cars
                .Where(c => c.IsAvailable)
                .OrderBy(c => c.PricePerday) // Optional: order by price
                .ToListAsync();

            return View(cars);
        }

        // GET: View customer's bookings
        public async Task<IActionResult> MyBookings()
        {
            var customerId = _temData.CustomerID;
            if (customerId == Guid.Empty)
                return BadRequest("Customer ID is invalid.");

            var bookings = await _dbContext.Bookings
                .Include(b => b.Car)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.PickupDate)
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
        public async Task<IActionResult> BookCar(Guid carId, DateTime pickupDate, DateTime returnDate)
        {
            var customerId = _temData.CustomerID;
            if (customerId == Guid.Empty)
                return BadRequest("Invalid customer.");

            if ((returnDate - pickupDate).Days <= 0)
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

            // Create booking
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                CarId = carId,
                CustomerId = (Guid)customerId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                TotalCost = totalCost
            };

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Bookings.Add(booking);

                    // Mark car as unavailable
                    car.IsAvailable = false;

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Booking failed. Please try again.");
                    return RedirectToAction(nameof(BookCar), new { carId });
                }
            }

            return RedirectToAction(nameof(MyBookings));
        }
    }
}
