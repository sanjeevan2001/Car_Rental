using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid(); // ✅ இதோ add பண்ணுங்க

        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
