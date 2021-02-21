
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ulvi.Models;
using Ulvi.ViewModels;

namespace Ulvi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            User appUser = await _userManager.FindByEmailAsync(login.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!!");
                return View(login);
            }
            if (!appUser.IsActivated)
            {
                ModelState.AddModelError("", "Email is Disabled!!");
                return View(login);
            }
            var signinResult = await _signInManager.PasswordSignInAsync(appUser, login.Password, true, true);
            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is wrong!!");
                return View(login);
            }
            if (signinResult.IsLockedOut)
            {
                ModelState.AddModelError("", "The account is locked Out");
                return View(login);
            }
            var role = (await _userManager.GetRolesAsync(appUser))[0];
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "User" });
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            User newUser = new User
            {
                Name = register.Name,
                UserName = register.UserName,
                Email = register.Email,
                IsActivated = true

            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            await _signInManager.SignInAsync(newUser, true);
            await _userManager.AddToRoleAsync(newUser, "Admin");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        public async Task CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            }
        }
    }
}
