using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class AdminCustomerController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCustomerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ✅ Admin Dashboard
        public IActionResult AdminDashBoard()
        {
            return View();
        }

        // ✅ List Customers
        public async Task<IActionResult> CustomerList()
        {
            var customers = await _context.Customers.Include(c => c.User).ToListAsync();
            return View(customers);
        }

        // ✅ GET: Add Customer
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        // ✅ POST: Add Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create User
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = model.UserName,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Role = "Customer"
                };

                // Create Customer
                var customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    UserEmail = model.UserEmail,
                    LicenseNumber = model.LicenseNumber,
                    UserId = user.UserId,
                    User = user
                };

                _context.Users.Add(user);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(CustomerList));
            }

            // If validation fails, return the same view
            return View(model);
        }


        // ✅ GET: Edit Customer
        [HttpGet]
        public async Task<IActionResult> EditCustomer(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // ✅ POST: Edit Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(Guid id, Customer customer)
        {
            if (id != customer.CustomerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (customer.User != null)
                    {
                        _context.Update(customer.User);
                    }
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(CustomerList));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(e => e.CustomerId == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(customer);
        }

        // ✅ GET: Customer Details
        [HttpGet]
        public async Task<IActionResult> CustomerDetails(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var customer = await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // ✅ GET: Delete Customer
        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        //  POST: Delete Customer
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer != null)
            {
                if (customer.User != null)
                {
                    _context.Users.Remove(customer.User);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CustomerList));
        }
    }
}



















//using CarRental.Data;
//using CarRental.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CarRental.Controllers
//{
//    public class AdminCustomerController : Controller
//    {
//        private readonly AppDbContext _context;

//        public AdminCustomerController(AppDbContext context)
//        {
//            _context = context;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }
//        // ✅ Admin Dashboard
//        public IActionResult AdminDashBoard()
//        {
//            return View();
//        }

//        // ✅ Show Customer List
//        public async Task<IActionResult> CustomerList()
//        {
//            var customers = await _context.Customers.ToListAsync();
//            return View(customers);
//        }

//        // ✅ GET: Add New Customer
//        [HttpGet]
//        public IActionResult AddCustomer()
//        {
//            return View();
//        }

//        // ✅ POST: Add New Customer
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddCustomer(Customer customer)
//        {
//            if (ModelState.IsValid)
//            {
//                customer.CustomerId = Guid.NewGuid();
//                _context.Customers.Add(customer);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(CustomerList));
//            }
//            return View(customer);
//        }

//        // ✅ GET: Edit Customer
//        [HttpGet]
//        public async Task<IActionResult> EditCustomer(Guid id)
//        {
//            if (id == Guid.Empty)
//                return NotFound();

//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null)
//                return NotFound();

//            return View(customer);
//        }

//        // ✅ POST: Edit Customer
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> EditCustomer(Guid id, Customer customer)
//        {
//            if (id != customer.CustomerId)
//                return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(customer);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(CustomerList));
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Customers.Any(e => e.CustomerId == id))
//                        return NotFound();
//                    else
//                        throw;
//                }
//            }
//            return View(customer);
//        }

//        // ✅ GET: Customer Details
//        [HttpGet]
//        public async Task<IActionResult> CustomerDetails(Guid id)
//        {
//            if (id == Guid.Empty)
//                return NotFound();

//            var customer = await _context.Customers
//                .Include(c => c.Bookings)
//                .FirstOrDefaultAsync(c => c.CustomerId == id);

//            if (customer == null)
//                return NotFound();

//            return View(customer);
//        }

//        // ✅ GET: Delete Customer (Confirmation Page)
//        [HttpGet]
//        public async Task<IActionResult> DeleteCustomer(Guid id)
//        {
//            if (id == Guid.Empty)
//                return NotFound();

//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null)
//                return NotFound();

//            return View(customer);
//        }

//        // ✅ POST: Delete Customer
//        [HttpPost, ActionName("DeleteCustomer")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteCustomerConfirmed(Guid id)
//        {
//            var customer = await _context.Customers.FindAsync(id);
//            if (customer != null)
//            {
//                _context.Customers.Remove(customer);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(CustomerList));
//        }

//    }
//}
