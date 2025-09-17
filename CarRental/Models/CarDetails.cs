using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class CarDetails
    {
        [Key]
        public int CarDetailId { get; set; }   //Guid
        [Required]
        public Guid CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }
        [Required]
        public int PricePerDay { get; set; }
        public string ExpDateOfLicense { get; set; }
        public string ExpDateOfTax { get; set; }

        public string ImageUrl { get; set; }
        public string Colour { get; set; }
        public bool IsAvailable { get; set; }


    }
}
