using Microsoft.AspNetCore.Identity;

namespace External.IdentityServerUI.Models
{
    public class AppUser : IdentityUser
    {
        public string Password { get; set; }
    }
}
