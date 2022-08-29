using System.ComponentModel.DataAnnotations;

namespace Blessings.Models
{
    public class SignUpModel
    {
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}
