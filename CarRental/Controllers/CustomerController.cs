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
        public IActionResult CustomerRegister(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            // Save customer
            _appdbcontext.Customers.Add(customer);
            _appdbcontext.SaveChanges();

            // Redirect to User registration with CustomerId
            return RedirectToAction("UserRegister", new { customerId = customer.CustomerId });
        }

        [HttpGet]
        public IActionResult UserRegister(Guid customerId)
        {
            var usermodel = new User {};
            ViewBag.CustomerId = customerId;
            return View(usermodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserRegister(User user, Guid customerId)
        {
            // The ConfirmPassword property is not mapped to the database.
            // To validate it, we need to explicitly check it.
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = customerId;
                return View(user);
            }

            // Find the customer by the provided customerId
            var customer = _appdbcontext.Customers.Find(customerId);
            if (customer == null)
            {
                // Handle the case where the customer isn't found
                ModelState.AddModelError(string.Empty, "Customer not found.");
                ViewBag.CustomerId = customerId;
                return View(user);
            }

            // Set the Role for the new user. You probably want to set this programmatically.
            user.Role = "Customer";

            // Assign a new GUID to the user if it's empty
            if (user.UserId == Guid.Empty)
            {
                user.UserId = Guid.NewGuid();
            }

            // Assign the UserId to the Customer to link them
            customer.UserId = user.UserId;

            // Set the Customer and User relationship
            user.Customers.Add(customer);

            // Add the new user to the context
            _appdbcontext.Users.Add(user);

            // Save changes to the database
            _appdbcontext.SaveChanges();

            TempData["SuccessMessage"] = "User and Customer registered successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
