

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Ulvi.Data;
using Ulvi.Models;

namespace Ulvi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly DataContext _db;
        public ProjectController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Projects.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid) return View();
            Project newProject = new Project();
            newProject.Name = project.Name;
            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Project project = _db.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Project project)
        {
            if (id == null) return NotFound();
            Project dbProject = await _db.Projects.FindAsync(id);
            if (dbProject == null) return NotFound();
            dbProject.Name = project.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Project project = await _db.Projects.FindAsync(id);
            if (project == null) return NotFound();
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
