namespace CarRental.Data
{
    public class TemData
    {
        // These properties will store the logged-in customer's info
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        // Default constructor
        public TemData()
        {
            CustomerID = null; // No customer logged in initially
            CustomerName = string.Empty;
        }
    }
}
