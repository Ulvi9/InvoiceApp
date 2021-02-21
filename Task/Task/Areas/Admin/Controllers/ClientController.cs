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
    public class ClientController : Controller
    {
        private readonly DataContext _db;
        public ClientController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Clients.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid) return View();
            Client newClient = new Client();
            newClient.Name = client.Name;
            newClient.Surname = client.Surname;
            await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Client client = _db.Clients.FirstOrDefault(x => x.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Client client)
        {
            if (id == null) return NotFound();
            Client dbClient = await _db.Clients.FindAsync(id);
            if (dbClient == null) return NotFound();
            dbClient.Name = client.Name;
            dbClient.Surname = client.Surname;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Client client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();
            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}