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



        // GET: /AdminCar/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null) return NotFound();

            var model = new CarViewModel
            {
                CarId = car.CarId,
                CarName = car.CarName,
                CarModel = car.CarModel,
                CarBrand = car.CarBrand,
                Seats = car.Seats,
                FuelType = car.FuelType,
                PricePerday = car.PricePerday,
                ImageUrl = car.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = _dbContext.Cars.Find(model.CarId);
                if (car == null) return NotFound();

                car.CarName = model.CarName;
                car.CarModel = model.CarModel;
                car.CarBrand = model.CarBrand;
                car.Seats = model.Seats;
                car.FuelType = model.FuelType;
                car.PricePerday = model.PricePerday;

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

                    car.ImageUrl = $"/images/Cars/{newFileName}{extension}";
                }

                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: /AdminCar/Delete/{id}
        public IActionResult Delete(Guid id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null) return NotFound();
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null) return NotFound();

            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /AdminCar/Details/{id}
        public IActionResult Details(Guid id)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.CarId == id);
            if (car == null) return NotFound();

            return View(car);
        }

    }
}
