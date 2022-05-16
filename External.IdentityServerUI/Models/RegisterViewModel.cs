using System.ComponentModel.DataAnnotations;

namespace External.IdentityServerUI.Models
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
