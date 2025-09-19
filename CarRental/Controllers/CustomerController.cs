using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _appdbcontext;

        public CustomerController(AppDbContext appDbContext)
        {
            _appdbcontext = appDbContext;
        }


        public IActionResult Index()
        {
            return View();
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
                Role = "Customer"
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
            return RedirectToAction("Index", "Home");
        }


        // Customer Register in Customer 
        [HttpGet]
        public IActionResult RegisterForCustomer() 
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterForCustomer(RegisterCustomerViewModel registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = registerVm.UserName,
                Password = registerVm.Password,
                Role = registerVm.Role
            };

             var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = registerVm.FullName,
                Gender = registerVm.Gender,
                Address = registerVm.Address,
                PhoneNumber = registerVm.PhoneNumber,
                UserEmail = registerVm.UserEmail,
                LicenseNumber = registerVm.LicenseNumber,
                UserId = user.UserId,
                User = user
            };

            _appdbcontext.Users.Add(user);
            _appdbcontext.Customers.Add(customer);
            _appdbcontext.SaveChanges();

            TempData["SuccessMessage"] = "Customer registered successfully!";
            return RedirectToPage("GuestView", "Guest");
        }

    }
}
