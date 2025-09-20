using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly AppDbContext _db;

        public AdminBookingController(AppDbContext db)
        {
            _db = db;
        }

        // GET: Booking List
        public async Task<IActionResult> Index()
        {
            var bookings = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Booking Details
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var booking = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Create Booking
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewData["CarId"] = new SelectList(_db.Cars.Where(c => c.IsAvailable), "CarId", "CarName");
            return View();
        }

        // POST: Create Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            // 1️⃣ Validate dates
            if (booking.ReturnDate <= booking.PickupDate)
            {
                ModelState.AddModelError("", "Return date must be later than pickup date.");
            }

            // 2️⃣ Check car availability before saving
            var car = await _db.Cars.FindAsync(booking.CarId);
            if (car == null || !car.IsAvailable)
            {
                ModelState.AddModelError("CarId", "Selected car is not available.");
            }

            if (!ModelState.IsValid)
            {
                // Reload dropdowns consistently
                ViewData["CustomerId"] = new SelectList(_db.Customers, "CustomerId", "FullName", booking.CustomerId);
                ViewData["CarId"] = new SelectList(
                    _db.Cars.Where(c => c.IsAvailable || c.CarId == booking.CarId),
                    "CarId", "CarName", booking.CarId
                );
                return View(booking);
            }

            // 3️⃣ Assign new BookingId
            booking.BookingId = Guid.NewGuid();

            // 4️⃣ Calculate total cost
            var days = (booking.ReturnDate - booking.PickupDate).Days;
            booking.TotalCost = days * car.PricePerday;

            // 5️⃣ Mark car as unavailable
            car.IsAvailable = false;

            // 6️⃣ Save booking
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Edit Booking
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_db.Customers, "CustomerId", "FullName", booking.CustomerId);
            ViewData["CarId"] = new SelectList(_db.Cars, "CarId", "CarName", booking.CarId);
            return View(booking);
        }

        // POST: Edit Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Recalculate cost if dates or car changed
                    var car = await _db.Cars.FindAsync(booking.CarId);
                    if (car != null)
                    {
                        var days = (booking.ReturnDate - booking.PickupDate).Days;
                        booking.TotalCost = days * car.PricePerday;
                    }

                    _db.Update(booking);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Bookings.Any(e => e.BookingId == booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_db.Customers, "CustomerId", "FullName", booking.CustomerId);
            ViewData["CarId"] = new SelectList(_db.Cars, "CarId", "CarName", booking.CarId);
            return View(booking);
        }

        // GET: Delete Booking
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var booking = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Delete Booking
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking != null)
            {
                // Free up the car again
                var car = await _db.Cars.FindAsync(booking.CarId);
                if (car != null)
                {
                    car.IsAvailable = true;
                }

                _db.Bookings.Remove(booking);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
