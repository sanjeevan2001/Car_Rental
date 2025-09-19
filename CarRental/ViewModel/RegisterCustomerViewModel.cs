using CarRental.Ennum;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModel
{
    public class RegisterCustomerViewModel
    {
        // ----- User Fields -----
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be 8-30 characters long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Customer";

        // ----- Customer Fields -----
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
