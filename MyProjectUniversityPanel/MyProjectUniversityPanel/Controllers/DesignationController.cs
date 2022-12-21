using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class DesignationController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DesignationController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Designation> designations = await _db.Designations.Where(x => !x.IsDeactive).ToListAsync();
            return View(designations);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Designation designation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (designation == null)
            {
                return BadRequest();
            }
            return View(designation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Designation designation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (designation == null)
            {
                return BadRequest();
            }
            designation.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Designation designation)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (designation.Name == null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty");
                return View();
            }
            bool isExist = await _db.Designations.Where(x => !x.IsDeactive).AnyAsync(x => x.Name == designation.Name && x.Name != null);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This designation is already exist");
                return View();
            }
            await _db.Designations.AddAsync(designation);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Designation dbDesignation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDesignation == null)
            {
                return BadRequest();
            }
            return View(dbDesignation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Designation designation)
        {
            if (id == null)
            {
                return NotFound();
            }
            Designation dbDesignation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDesignation == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbDesignation);
            }
            bool isExist = await _db.Designations.Where(x => !x.IsDeactive).AnyAsync(x => x.Name == designation.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "This category is already exist");
                return View(dbDesignation);
            }
            dbDesignation.Name = designation.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
