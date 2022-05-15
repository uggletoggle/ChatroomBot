using External.IdentityServerUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace External.IdentityServerUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl =returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginVm)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVm.UserName, loginVm.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(loginVm.ReturnUrl);
            }

            return View();
        }
    }
}
