using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Ulvi.Data;

namespace Ulvi.Areas.Admin.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly DataContext _db;
        public InvoiceController(DataContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Invoices.ToList());
        }
    }
}
