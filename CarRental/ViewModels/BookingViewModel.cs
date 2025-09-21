using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class BookingViewModel
    {
        public Guid BookingId { get; set; }

        [Required(ErrorMessage = "Pickup date is required")]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Return date is required")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(1);


        //[Required(ErrorMessage = "Pickup date is required")]
        //[DataType(DataType.Date)]
        //public DateTime PickupDate { get; set; }

        //[Required(ErrorMessage = "Return date is required")]
        //[DataType(DataType.Date)]
        //public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Car is required")]
        public Guid CarId { get; set; }

        public decimal TotalCost { get; set; }

        // For dropdowns
        public string CustomerName { get; set; }
        public string CarName { get; set; }
    }
}
