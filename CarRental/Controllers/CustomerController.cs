using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _appdbcontext;

        public CustomerController(AppDbContext appDbContext)
        {
            _appdbcontext = appDbContext;
        }

        [HttpGet]
        public IActionResult CustomerRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRegister(CarRental.ViewModel.UserCustomer model)
        {

            if (!ModelState.IsValid)
            {
                // Return view with validation errors
                return View(model);
            }

            // 1. Create User object from ViewModel
            var user = new User
            {
                UserName = model.UserName,

                Password = model.Password,
                Role = "Customer",
                PhoneNumber = model.PhoneNumber
            };

            // 2. Save User to DB
            _appdbcontext.Users.Add(user);
            await _appdbcontext.SaveChangesAsync();


            // 3. Create Customer object from ViewModel
            var customer = new Customer
            {
                FullName = model.FullName,
                Gender = model.Gender,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                UserEmail = model.UserEmail,
                LicenseNumber = model.LicenseNumber,
                UserId = user.UserId  // link to User
            };

            // 4. Save Customer to DB
            _appdbcontext.Customers.Add(customer);
            await _appdbcontext.SaveChangesAsync();

            // 5. Redirect or show success message
            return RedirectToAction("CustomerList", "AdminCustomer");

        }

        // User Register Form
        [HttpGet]
        public IActionResult UserRegister()
        {
            return View();
        }

        // POST: Save User for Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegister(CustomerSignupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if username already exists
            var existingUser = await _appdbcontext.Users
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);

            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "Username is already taken.");
                return View(model);
            }

            // Hash the password 
            string hashedPassword = model.Password;

            // Create new User
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = model.UserName,
                Password = hashedPassword,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber
            };

            _appdbcontext.Users.Add(user);

            await _appdbcontext.SaveChangesAsync();

            TempData["SuccessMessage"] = "User registered successfully!";
            return RedirectToAction("RegisterForCustomer", "Customer");
        }



        [HttpGet]
        public IActionResult RegisterForCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterForCustomer(RegisterCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if Phone Number exists in Users table
            var user = _appdbcontext.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);

            if (user == null)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is not registered as a User.");
                return View(model);
            }

            // Check if already registered as Customer
            var existingCustomer = _appdbcontext.Customers.FirstOrDefault(c => c.PhoneNumber == model.PhoneNumber);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already registered as a Customer.");
                return View(model);
            }

            // Map ViewModel → Customer
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = model.FullName,
                Gender = model.Gender,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                UserEmail = model.UserEmail,
                LicenseNumber = model.LicenseNumber,
                UserId = user.UserId
            };

            _appdbcontext.Customers.Add(customer);
            _appdbcontext.SaveChanges();

            // Otherwise, go to home/dashboard
            TempData["Success"] = "Customer registration successful!";
            return RedirectToAction("Index", "CustomerDashboard");
        }
    }
}
