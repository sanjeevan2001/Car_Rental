using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class Car
    {
        public Guid CarId { get; set; }

        [Required, MaxLength(100)]
        public string CarName { get; set; }

        [Required, MaxLength(50)]
        public string CarModel { get; set; }
        [Required, MaxLength(50)]
        public string CarBrand { get; set; }
        [Required, MaxLength(50)]
        public string FuelType { get; set; }
        [Required, MaxLength(50)]
        public string Colour { get; set; }

        [MaxLength(200)]
        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; }

    }
}
