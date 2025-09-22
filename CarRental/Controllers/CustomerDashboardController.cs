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
            // Customer name
            ViewBag.CustomerName = _temData.CustomerName;

            // Show only top 3 available cars as "featured"
            var featuredCars = _dbContext.Cars
                .Where(c => c.IsAvailable)
                .OrderBy(c => c.PricePerday)   // cheapest cars first
                .Take(3)
                .ToList();

            ViewBag.FeaturedCars = featuredCars;

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
            if (pickupDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("PickupDate", "Pickup date must be today or later.");
            }

            if (returnDate.Date <= pickupDate.Date)
            {
                ModelState.AddModelError("ReturnDate", "Return date must be after Pickup date.");
            }
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

        // GET: Customer Profile
        public async Task<IActionResult> CustomerProfile()
        {
            // Check if customer is logged in
            if (!_temData.CustomerID.HasValue || _temData.CustomerID.Value == Guid.Empty)
                return RedirectToAction("GuestView", "Guest");

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == _temData.CustomerID.Value);
            

            if (customer == null)
                return NotFound("Customer not found. ");

            return View(customer);
        }

        // GET: Edit Customer Profile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (!_temData.CustomerID.HasValue || _temData.CustomerID.Value == Guid.Empty)
                return RedirectToAction("GuestView", "Guest");

            var customer = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == _temData.CustomerID.Value);

            if (customer == null)
                return NotFound("Customer not found.");

            return View(customer);
        }

        // POST: Edit Customer Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == model.CustomerId);

            if (customer == null)
                return NotFound("Customer not found.");

            // Update fields safely
            customer.FullName = model.FullName?.Trim() ?? customer.FullName;
            customer.Gender = model.Gender;
            customer.Address = model.Address?.Trim() ?? customer.Address;
            customer.UserEmail = model.UserEmail?.Trim() ?? customer.UserEmail;
            customer.PhoneNumber = model.PhoneNumber?.Trim() ?? customer.PhoneNumber;
            customer.LicenseNumber = model.LicenseNumber?.Trim() ?? customer.LicenseNumber;

            await _dbContext.SaveChangesAsync();

            // Update TempData
            _temData.CustomerName = customer.FullName;

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("CustomerProfile"); // Make sure this action exists
        }


    }
}
