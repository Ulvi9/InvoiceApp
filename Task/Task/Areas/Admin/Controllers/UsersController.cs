using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ulvi.Data;
using Ulvi.Models;
using Ulvi.ViewModels;

namespace Ulvi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _db;
        private readonly UserManager<User> _userManager;
        public UsersController(DataContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult Index(string name)
        {
            var users = (name == null) ? _userManager.Users.Where(x => x.IsActivated == true).ToList() :
                _userManager.Users.Where(x => x.IsActivated == true && x.Name.ToLower().Contains(name.ToLower())).ToList();

            return View(users);
        }
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                UserRoleVM userRoleVM = new UserRoleVM
                {
                    User = user,
                    UserRoles = userRoles

                };

                return View(userRoleVM);

            }

            return NotFound();

        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            User newUser = new User
            {
                Name = register.Name,
                Email = register.Email,
                UserName = register.UserName
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
            newUser.EmailConfirmed = true;
            newUser.IsActivated = true;

            await _userManager.AddToRoleAsync(newUser, "User");

            TempData["success"] = "User Created";

            return RedirectToAction("Index", "Users");

        }


        public async Task<IActionResult> IsActivate(string id)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsActivate(string id, bool IsActivated)
        {
            if (id == null) return NotFound();
            User existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            if (existUser.Id!=user.Id)
            {
                user.IsActivated = IsActivated;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteList()
        {
            return View(_userManager.Users.Where(d => d.IsActivated == false));
        }

        public IActionResult UpdateProfile(string id)
        {
            if (id == null) return NotFound();
            User user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string id, User user)
        {
            if (!ModelState.IsValid) return View();
            var dbUser = _userManager.Users.FirstOrDefault(x => x.Id == id);
            
         
            if (user.Email != dbUser.Email)
            {
                if (_db.Users.Any(x => x.Email == user.Email))
                {
                    return NotFound(new { Message = "Agillisanda?!" });
                }
            }
            dbUser.Email = user.Email;
            dbUser.NormalizedEmail = user.Email;
            dbUser.Name = user.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string Password)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null) return NotFound();
            User user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            User appUser = _db.Users.FirstOrDefault(s => s.Id == id);
            User existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (appUser == null) return NotFound();
            if (appUser.Id != existUser.Id)
            {
                _db.Users.Remove(appUser);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
