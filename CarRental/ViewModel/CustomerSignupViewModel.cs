using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModel
{
    public class CustomerSignupViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100)]
        public string UserName { get; set; }   

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }   

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
    }
}
