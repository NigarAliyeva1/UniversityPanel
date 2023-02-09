using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using Microsoft.AspNetCore.Authorization;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DepartmentsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department department = await _db.Departments.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (department == null)
            {
                return BadRequest();
            }
            return View(department);
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
            Department department = await _db.Departments.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (department == null)
            {
                return BadRequest();
            }
            department.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (department.Name==null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty");
                return View();
            }
            bool isExist = await _db.Departments.Where(x => !x.IsDeactive).AnyAsync(x => x.Name == department.Name && x.Name!=null);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This department is already exist");
                return View();
            }
            await _db.Departments.AddAsync(department);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department dbDepartment = await _db.Departments.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDepartment == null)
            {
                return BadRequest();
            }
            return View(dbDepartment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Department department)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department dbDepartment = await _db.Departments.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDepartment == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbDepartment);
            }
            bool isExist = await _db.Departments.Where(x => !x.IsDeactive).AnyAsync(x => x.Name == department.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "This department is already exist");
                return View(dbDepartment);
            }
            dbDepartment.Name = department.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Detail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Department department = await _db.Departments.FirstOrDefaultAsync(x => x.Id == id);
        //    if (department == null)
        //    {
        //        return BadRequest();
        //    }
        //    return View(department);
        //}
    }
}
