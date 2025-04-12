using System.ComponentModel.DataAnnotations;

namespace Vendor.Models
{
    public class SuperAdminLogin
    {
        [Key]
        public int SuperAdminId { get; set; }

        [StringLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}