using CarRental.Ennum;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModel
{
    public class RegisterCustomerViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public CustomerGender Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(?:\+94|0)?[0-9]{9}$",
            ErrorMessage = "Invalid Sri Lanka phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "License Number is required")]
        [RegularExpression(@"^[A-Za-z0-9\-\/]{5,20}$",
            ErrorMessage = "License Number must be 5-20 characters long (letters, numbers, - or / allowed)")]
        public string LicenseNumber { get; set; }
    }
}
