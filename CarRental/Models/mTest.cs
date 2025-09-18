using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class mTest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string logo { get; set; }
    }
}
