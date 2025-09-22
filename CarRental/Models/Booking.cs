using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRental.CustomValidation;

namespace CarRental.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Pickup Date")]
        [CustomValidation(typeof(Booking), nameof(ValidatePickupDate))]
        public DateTime PickupDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
       
        public DateTime ReturnDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalCost { get; set; }

        

        [Required]
        //[ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        //[ForeignKey("CarId")]
        public Guid CarId { get; set; }

        public Car Car { get; set; }

        // Custom validation method for PickupDate
        public static ValidationResult ValidatePickupDate(DateTime pickupDate, ValidationContext context)
        {
            if (pickupDate.Date < DateTime.Today)
            {
                return new ValidationResult("Pickup date must be today or later.");
            }
            return ValidationResult.Success;
        }
        // Return Date must be after Pickup Date
        public static ValidationResult ValidateReturnDate(DateTime returnDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Booking;
            if (instance == null)
                return ValidationResult.Success;

            if (returnDate.Date <= instance.PickupDate.Date)
            {
                return new ValidationResult("Return date must be after Pickup date.");
            }

            return ValidationResult.Success;
        }
    }
}
