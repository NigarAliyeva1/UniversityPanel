using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.tool.xml.html.HTML;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class DepartmentsDetailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public DepartmentsDetailController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<DepartmentDetail> departmentDetails = await _db.DepartmentDetails.Include(x => x.Teacher).Include(x => x.Department).ToListAsync();
            return View(departmentDetails);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentDetail departmentDetail, int? depId, int? techId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (departmentDetail.Phone == null)
            {
                ModelState.AddModelError("Phone", "This field cannot be empty");
                return View();
            }
            if (departmentDetail.Email == null)
            {
                ModelState.AddModelError("Email", "This field cannot be empty");
                return View();
            }
            
            departmentDetail.DepartmentId = (int)depId;
            departmentDetail.TeacherId = (int)techId;
            await _db.DepartmentDetails.AddAsync(departmentDetail);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            DepartmentDetail dbDepartmentDetail = await _db.DepartmentDetails.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDepartmentDetail == null)
            {
                return BadRequest();
            }
            return View(dbDepartmentDetail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, DepartmentDetail departmentDetail, int? depId, int? techId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
           
            DepartmentDetail dbDepartmentDetail = await _db.DepartmentDetails.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDepartmentDetail == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbDepartmentDetail);
            }
            bool isExist = await _db.DepartmentDetails.Where(x => !x.IsDeactive).AnyAsync(x => x.Email == departmentDetail.Email && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Email", "This email is already exist");
                return View(dbDepartmentDetail);
            }
            dbDepartmentDetail.Email = departmentDetail.Email;
            dbDepartmentDetail.Phone= departmentDetail.Phone;
            dbDepartmentDetail.Capacity= departmentDetail.Capacity;
            dbDepartmentDetail.DepartmentId = (int)depId;
            dbDepartmentDetail.TeacherId = (int)techId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DepartmentDetail DepartmentDetail = await _db.DepartmentDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (DepartmentDetail == null)
            {
                return BadRequest();
            }
            if (DepartmentDetail.IsDeactive)
            {
                DepartmentDetail.IsDeactive = false;
            }
            else
            {
                DepartmentDetail.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
