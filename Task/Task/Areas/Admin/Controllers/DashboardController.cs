using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ulvi.Data;
using Ulvi.Models;

namespace Ulvi.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _db;
        public DashboardController(UserManager<User> userManager, DataContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = user;
            }
            return View();
        }
    }
}