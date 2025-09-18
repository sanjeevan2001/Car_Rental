using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalCost { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [ForeignKey("CarId")]
        public Guid CarId { get; set; }

        public Car Car { get; set; }
    }
}
