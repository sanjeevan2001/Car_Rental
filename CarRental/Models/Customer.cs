using CarRental.Ennum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Fullname is Required")]
        public string FullName { get; set; }

        [Required]
        public CustomerGender Gender { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phonenumber is Required")]
        [Phone(ErrorMessage = "Invalid Phonenumber format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Full Name is Required")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "License Number is required")]
        [RegularExpression(@"^[A-Za-z0-9]{5,20}$",
        ErrorMessage = "License Number must be 5-20 characters long and contain only letters and numbers")]
        [StringLength(20, MinimumLength = 5,
        ErrorMessage = "License Number should be between 5 and 20 characters")]
        public string LicenseNumber { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User User { get; set; }

        public ICollection<Booking> Bookings { get; set; }

    }
}
