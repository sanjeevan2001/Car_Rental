using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CarViewModel
    {
        public Guid CarId { get; set; }
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
        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000")]
        public decimal PricePerday { get; set; }   // <-- added
        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }  // <-- handles upload
        [Display(Name = "Availability")]
        public bool IsAvailable { get; set; } = true; // default

    }
}
