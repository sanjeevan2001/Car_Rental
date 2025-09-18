using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public Guid BookingId { get; set; }

        public Booking Booking { get; set; }

    }
}
