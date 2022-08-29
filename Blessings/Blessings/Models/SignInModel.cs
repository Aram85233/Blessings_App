using System.ComponentModel.DataAnnotations;

namespace Blessings.Models
{
    public class SignInModel
    {
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
