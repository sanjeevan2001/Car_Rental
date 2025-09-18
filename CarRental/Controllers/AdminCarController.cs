using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminCarController : Controller
    {
        public AppDbContext _dbContext;
        public IWebHostEnvironment _webHostEnvironment;

        public AdminCarController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var cars = _dbContext.Cars.ToList();
            return View(cars);
        }


        [HttpGet]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCar(CarViewModel model)
        {
           

            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;

                if (model.ImageFile != null)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string newFileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(webRootPath, "images", "Cars");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadPath, newFileName + extension);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    imagePath = $"/images/Cars/{newFileName}{extension}";
                }

                var car = new Car
                {
                    
                    CarName = model.CarName,
                    CarModel = model.CarModel,
                    CarBrand = model.CarBrand,
                    Seats = model.Seats,
                    FuelType = model.FuelType,
                    PricePerday = model.PricePerday,
                    ImageUrl = imagePath,
                    IsAvailable = true // default
                };

                _dbContext.Cars.Add(car);
                _dbContext.SaveChanges();
                return RedirectToAction("Index","AdminCar");
            }

            return View(model);
        }
        public IActionResult Edit()
        {
            
            return View();
        }
        public IActionResult Delete()
        {

            return View();
        }
        public IActionResult Details()
        {

            return View();
        }
    }
}
