using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class TestCarController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _dbContext;

        public TestCarController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(mTest test)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file != null && file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, "images", "Cars");

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);
                }

                var extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                test.logo = "/images/Cars/" + newFileName + extension;

                TempData["Message"] = $"Car Added: {test.Name}, Logo Path: {test.logo}";
            }

            if (ModelState.IsValid)
            {
                
                _dbContext.SaveChanges();
                return RedirectToAction("Car");
            }


            return View();
        }
    }
}
