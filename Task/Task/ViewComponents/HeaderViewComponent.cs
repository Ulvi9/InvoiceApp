using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Ulvi.Data;
using Ulvi.Models;

namespace Ulvi.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly DataContext _db;
        private readonly UserManager<User> _userManager;
        public HeaderViewComponent(DataContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bio model = _db.Bios.FirstOrDefault();
            ViewBag.User = "";
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = user;
            }
            return View(await Task.FromResult(model));
        }
    }
}
