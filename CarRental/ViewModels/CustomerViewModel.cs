using CarRental.Ennum;
using CarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CustomerViewModel
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public Ennum.CustomerGender Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LicenseNumber { get; set; }


    }
}
