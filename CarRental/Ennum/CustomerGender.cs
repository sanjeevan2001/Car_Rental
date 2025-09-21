using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Ennum
{
    public enum CustomerGender 
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,
        
        [Display(Name = "Others")]
        Others = 3
    }
}
