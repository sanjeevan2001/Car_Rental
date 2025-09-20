using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly AppDbContext _context;

        public AdminBookingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .ToListAsync();

            var viewModel = bookings.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                PickupDate = b.PickupDate,
                ReturnDate = b.ReturnDate,
                CustomerId = b.CustomerId,
                CarId = b.CarId,
                TotalCost = b.TotalCost,
                CustomerName = b.Customer?.FullName,
                CarName = b.Car?.CarName
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewBag.Customers = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewBag.Cars = new SelectList(_context.Cars.Where(c => c.IsAvailable), "CarId", "CarName");
            return View(new BookingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FindAsync(model.CarId);
                if (car == null || !car.IsAvailable)
                {
                    ModelState.AddModelError("CarId", "Selected car is not available.");
                }
                else
                {
                    var booking = new Booking
                    {
                        BookingId = Guid.NewGuid(),
                        PickupDate = model.PickupDate,
                        ReturnDate = model.ReturnDate,
                        CustomerId = model.CustomerId,
                        CarId = model.CarId,
                        TotalCost = (decimal)(model.ReturnDate - model.PickupDate).TotalDays * car.PricePerday
                    };

                    car.IsAvailable = false;

                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload dropdowns if validation fails
            ViewBag.Customers = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewBag.Cars = new SelectList(_context.Cars.Where(c => c.IsAvailable), "CarId", "CarName");
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var model = new BookingViewModel
            {
                BookingId = booking.BookingId,
                PickupDate = booking.PickupDate,
                ReturnDate = booking.ReturnDate,
                CustomerId = booking.CustomerId,
                CarId = booking.CarId,
                TotalCost = booking.TotalCost
            };

            ViewBag.Customers = new SelectList(_context.Customers, "CustomerId", "FullName", model.CustomerId);
            ViewBag.Cars = new SelectList(_context.Cars, "CarId", "CarName", model.CarId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BookingViewModel model)
        {
            if (id != model.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null) return NotFound();

                var car = await _context.Cars.FindAsync(model.CarId);

                booking.PickupDate = model.PickupDate;
                booking.ReturnDate = model.ReturnDate;
                booking.CustomerId = model.CustomerId;
                booking.CarId = model.CarId;
                booking.TotalCost = (decimal)(model.ReturnDate - model.PickupDate).TotalDays * car.PricePerday;

                _context.Update(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = new SelectList(_context.Customers, "CustomerId", "FullName", model.CustomerId);
            ViewBag.Cars = new SelectList(_context.Cars, "CarId", "CarName", model.CarId);
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            var model = new BookingViewModel
            {
                BookingId = booking.BookingId,
                PickupDate = booking.PickupDate,
                ReturnDate = booking.ReturnDate,
                TotalCost = booking.TotalCost,
                CustomerName = booking.Customer.FullName,
                CarName = booking.Car.CarName
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var car = await _context.Cars.FindAsync(booking.CarId);
            if (car != null) car.IsAvailable = true;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

