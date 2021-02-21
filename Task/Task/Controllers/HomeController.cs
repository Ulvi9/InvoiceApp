using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ulvi.Data;
using Ulvi.Models;
using Ulvi.ViewModels;

namespace Ulvi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DataContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(DataContext db,
                UserManager<User> userManager,
                SignInManager<User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string name)
        {
            HomeVM homeVM = new HomeVM();            
            User existuser = await _userManager.FindByNameAsync(User.Identity.Name);

            homeVM.Invoices = (name == null) ? _db.Invoices
                .Where(x => x.UserId == existuser.Id && x.IsDeleted == false)
                .Include(x => x.Project)
                .Include(x => x.Client).ToList() :
                _db.Invoices
                .Where(x => x.UserId == existuser.Id && x.IsDeleted == false && 
                x.Project.Name.ToLower().Contains(name.ToLower()))
                .Include(x => x.Project)
                .Include(x => x.Client).ToList();
            return View(homeVM);
        }
        public IActionResult FilterDateTime(DateTime startDate,DateTime endTime)
        {
            var invoices = _db.Invoices
                .Where(x => x.InvoiceDate >= startDate && x.InvoiceDate <= endTime).ToList();
            return RedirectToAction("index", "Home", new HomeVM { Invoices = invoices });

        }
        public async Task<IActionResult> DeletedList()
        {
            User existuser = await _userManager.FindByNameAsync(User.Identity.Name);
            var invoices = _db.Invoices
                .Where(x => x.UserId == existuser.Id && x.IsDeleted == true)
                .Include(x => x.Project)
                .Include(x => x.Client).ToList();


            return View(invoices);
        }


    }
}
