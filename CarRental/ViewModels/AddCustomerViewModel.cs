using CarRental.Ennum;

namespace CarRental.ViewModels
{
    public class AddCustomerViewModel
    {// Customer fields
        public string FullName { get; set; }
        public CustomerGender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string LicenseNumber { get; set; }

        // User credentials
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
