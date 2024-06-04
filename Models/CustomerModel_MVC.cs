using System.ComponentModel.DataAnnotations;

namespace E_Commerce_MVC.Models
{
    public class CustomerModel_MVC
    {
        public int Customer_Id { get; set; }
        [Required(ErrorMessage = "First_Name Field is Required..")]
        public String First_Name { get; set; }
        public String Last_Name { get; set; }

        [Required(ErrorMessage = "Gender Field is Required..")]
        public String Gender { get; set; }

        [Required(ErrorMessage = "Email Field is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address..")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter a correct email")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Mobile Field is Required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$")]
        public String Mobile { get; set; }
        
        public String? Interested_Category { get; set; }

        public List<String>? SelectedInterested_Category { get; set; }

        public CustomerModel_MVC()
        {
            SelectedInterested_Category = new List<String>();
        }

    }
}
