using Microsoft.AspNetCore.Identity;

namespace External.IdentityServer.Models
{
    public class AppUser : IdentityUser
    {
        public string Password { get; set; }
    }
}
