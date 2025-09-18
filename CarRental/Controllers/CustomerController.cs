using CarRental.Data;
using CarRental.Models;
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
            model.Gender = Ennum.CustomerGender.Female;

            if (!ModelState.IsValid)
            {
                // Return view with validation errors
                return View(model);
            }

            // 1. Create User object from ViewModel
            var user = new User
            {
                UserName = model.UserName,
                Password = model.Password, // Consider hashing in real apps!
                Role = "Customer"           // Set default role as Customer
            };

            // 2. Save User to DB
            _appdbcontext.Users.Add(user);
            await _appdbcontext.SaveChangesAsync();

            // user.UserId is now populated

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
            return RedirectToAction("Index", "Home"); // or wherever you want
        }


        //[HttpGet]
        //public IActionResult UserRegister(Guid customerId)
        //{
        //    var usermodel = new User {};
        //    ViewBag.CustomerId = customerId;
        //    return View(usermodel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult UserRegister(User user, Guid customerId)
        //{
        //    // Role assign பண்ணுறது ModelState validate செய்ய முன்
        //    user.Role = "Customer";

        //    // Confirm password validation
        //    if (user.Password != user.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.CustomerId = customerId;
        //        return View(user);
        //    }

        //    // Find the customer
        //    var customer = _appdbcontext.Customers.Find(customerId);
        //    if (customer == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Customer not found.");
        //        ViewBag.CustomerId = customerId;
        //        return View(user);
        //    }

        //    // Link both sides
        //    customer.User = user;
        //    user.Customers = new List<Customer> { customer };

        //    _appdbcontext.Users.Add(user);
        //    _appdbcontext.SaveChanges();

        //    TempData["SuccessMessage"] = "User and Customer registered successfully!";
        //    return RedirectToAction("Index", "Home");
        //}


    }
}
