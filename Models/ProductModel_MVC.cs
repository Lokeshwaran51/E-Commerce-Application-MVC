using System.ComponentModel.DataAnnotations;

namespace E_Commerce_MVC.Models
{
    public class ProductModel_MVC
    {
        public String ProductCategory { get; set; }
        [Required]
        public int Product_Code { get; set; }
        [Required]
        public String Product_Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public String Product_Description { get; set; }
        [Required]
        public int Available_Qty { get; set; }
    }
}
