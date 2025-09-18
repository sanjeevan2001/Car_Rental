using CarRental.Data;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        public AppDbContext _dbContext { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }

        public CarController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
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
            //string webRootPath = _webHostEnvironment.WebRootPath;
            //var file = HttpContext.Request.Form.Files;
            //if (file != null && file.Count > 0)
            //{
            //    string newFileName = Guid.NewGuid().ToString();
            //    var upload = Path.Combine(webRootPath, "images", "Cars");

            //    if (!Directory.Exists(upload))
            //    {
            //        Directory.CreateDirectory(upload);
            //    }

            //    var extension = Path.GetExtension(file[0].FileName);
            //    using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
            //    {
            //        file[0].CopyTo(fileStream);
            //    }
            //    car.ImageUrl = @"/images/Cars/" + newFileName + extension;
            //}

            //if (ModelState.IsValid)
            //{
            //    _dbContext.Cars.Add(car);
            //    _dbContext.SaveChanges();
            //    return RedirectToAction(nameof(Index));
            //}


            //return View(car);

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
                    CarId = Guid.NewGuid(),
                    CarName = model.CarName,
                    CarModel = model.CarModel,
                    CarBrand = model.CarBrand,
                    Seats = model.Seats,
                    FuelType = model.FuelType,
                    ImageUrl = imagePath,
                    IsAvailable = true // default
                };

                _dbContext.Cars.Add(car);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
