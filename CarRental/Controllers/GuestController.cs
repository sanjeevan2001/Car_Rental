using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class GuestController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public GuestController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GuestView()
        {
            var cars = _appDbContext.Cars.ToList();
            return View(cars);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var feedback = new Feedback
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    CreatedAt = DateTime.UtcNow
                };

                _appDbContext.Feedbacks.Add(feedback);
                _appDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Thank you for your feedback! We'll get back to you soon.";
                return RedirectToAction("Contact");
            }

            return View("Contact", model);
        }

     
       
        public IActionResult news1()
        {
            return View();
        }
    }
}
