using External.IdentityServerUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace External.IdentityServerUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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

            ViewBag.Error = "Invalid credentials";
            return View(new LoginViewModel { ReturnUrl = loginVm.ReturnUrl });
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }

            var user = new AppUser();
            user.UserName = registerVm.UserName;

            var result = await _userManager.CreateAsync(user, registerVm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("username", user.UserName));
                await _signInManager.SignInAsync(user, false);
                return Redirect(registerVm.ReturnUrl);
            }

            ViewBag.Error = "Invalid credentials";
            return View(new LoginViewModel { ReturnUrl = registerVm.ReturnUrl });
        }
    }
}
