using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CarViewModel
    {
        [Required, MaxLength(100)]
        public string CarName { get; set; }

        [Required, MaxLength(50)]
        public string CarModel { get; set; }

        [Required, MaxLength(50)]
        public string CarBrand { get; set; }

        [Required]
        public string Seats { get; set; }

        [Required]
        public string FuelType { get; set; }

        [Required]
        public decimal PricePerday { get; set; }   // <-- added

        public IFormFile? ImageFile { get; set; }  // <-- handles upload
        
    }
}
