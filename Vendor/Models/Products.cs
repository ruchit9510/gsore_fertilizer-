using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vendor.Models
{
    public class Products
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product Price must be greater than zero.")]
        public int ProductPrice { get; set; }

        public string ProductPicture { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }

        public List<Category> Categories { get; set; }
    }
}
