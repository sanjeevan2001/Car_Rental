using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly TemData _temData;

        public LoginController(AppDbContext dbContext, TemData temService)
        {
            _dbContext = dbContext;
            _temData = temService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and Password are required";
                return View("Index");
            }

            // Find the customer by username and password
            var customer = await _dbContext.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.UserName == username && c.User.Password == password);

            if (username == "Admin" && password == "Admin123")
            {

                // Redirect to home or dashboard
                return RedirectToAction("Index", "AdminDashboard");
            }




            if (customer != null)
            {
                // Store customer info in TemData
                _temData.CustomerID = customer.CustomerId;
                _temData.CustomerName = customer.FullName;

                // Redirect to home or dashboard
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
                return View("Index");
            }
        }

        // Optional: Logout
        //public IActionResult Logout()
        //{
        //    _temService.CustomerId = null;
        //    _temService.CustomerName = null;
        //    return RedirectToAction("Index");
        //}
    }
}
