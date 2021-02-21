using Microsoft.AspNetCore.Authorization;
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
    public class InvoiceController : Controller
    {
        private readonly DataContext _db;
        private readonly UserManager<User> _userManager;
        public InvoiceController(DataContext db,UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            ViewBag.Client = _db.Clients.ToList();
            ViewBag.Project = _db.Projects.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceVM createInvoice)
        {
            User existuser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Client = _db.Clients.ToList();
            ViewBag.Project = _db.Projects.ToList();
            if (!ModelState.IsValid) return View();
           
            Invoice newInvoice = new Invoice();
            newInvoice.InvoiceNumber = createInvoice.InvoiceNumber;
            newInvoice.InvoiceDate = createInvoice.InvoiceDate;
            newInvoice.ClientId = createInvoice.ClientId;
            newInvoice.NetAmount = createInvoice.NetAmount;
            newInvoice.TaxAmount = createInvoice.TaxAmount;
            newInvoice.TotalAmount = (createInvoice.TaxAmount * createInvoice.NetAmount) / 100 + createInvoice.NetAmount;
            newInvoice.ProjectId = createInvoice.ProjectId;
            newInvoice.InvoiceNote = createInvoice.InvoiceNote;
            newInvoice.UserId = existuser.Id;
            newInvoice.Name = createInvoice.Name;
            newInvoice.InvoiceDate =DateTime.Now;
            if (createInvoice.PaymentStatus!=0)
            {
                newInvoice.PaymentStatus = Enum.PaymentStatus.Done;
            }
            await _db.Invoices.AddAsync(newInvoice);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Updates(int? id)
        {
            ViewBag.Client = _db.Clients.ToList();
            ViewBag.Project = _db.Projects.ToList();

            if (id == null) return NotFound();
            Invoice dbInvoice = await _db.Invoices.FirstOrDefaultAsync(x=>x.Id==id);
            if (dbInvoice == null) return NotFound();
            return View(dbInvoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Updates(UpdateInvoiceVM updateInvoiceVM)
        {
            ViewBag.Client = _db.Clients.ToList();
            ViewBag.Project = _db.Projects.ToList();
            if (!ModelState.IsValid) return View();
            Invoice dbInvoice = await _db.Invoices
               .Include(x => x.Project)
               .Include(x => x.Client).Where(x=>x.Id==updateInvoiceVM.Id).FirstOrDefaultAsync();
            if (dbInvoice == null) return NotFound();
            dbInvoice.InvoiceNumber = updateInvoiceVM.InvoiceNumber;
            dbInvoice.InvoiceDate = updateInvoiceVM.InvoiceDate;
            dbInvoice.ClientId = updateInvoiceVM.ClientId;
            dbInvoice.NetAmount = updateInvoiceVM.NetAmount;
            dbInvoice.TaxAmount = updateInvoiceVM.TaxAmount;
            dbInvoice.TotalAmount = (updateInvoiceVM.TaxAmount * updateInvoiceVM.NetAmount) / 100 + updateInvoiceVM.NetAmount;
            dbInvoice.ProjectId = updateInvoiceVM.ProjectId;
            dbInvoice.InvoiceDate = DateTime.Now;
            dbInvoice.InvoiceNote =updateInvoiceVM.InvoiceNote;
            dbInvoice.Name = updateInvoiceVM.Name;
            if (updateInvoiceVM.PaymentStatus != 0)
            {
                updateInvoiceVM.PaymentStatus = (int)Enum.PaymentStatus.Done;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Invoice dbInvoice = await _db.Invoices
                .Include(x => x.Project)
                .Include(x => x.Client).FirstOrDefaultAsync(x=>x.Id==id);
            dbInvoice.IsDeleted = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }
    }
}
