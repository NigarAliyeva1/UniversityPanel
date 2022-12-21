using iTextSharp.text;
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
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class TeacherGroupsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public TeacherGroupsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<TeacherGroups> teacherGroups = await _db.TeacherGroups.Include(x => x.Group).Include(x => x.Teacher).ToListAsync();
            return View(teacherGroups);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherGroups teacherGroups, int? groupId, int? techId)
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (groupId == null || techId == null)
            {
                return View();
            }
            teacherGroups.GroupId = (int)groupId;
            teacherGroups.TeacherId = (int)techId;
            await _db.TeacherGroups.AddAsync(teacherGroups);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            TeacherGroups dbTeacherGroups = await _db.TeacherGroups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacherGroups == null)
            {
                return BadRequest();
            }
            return View(dbTeacherGroups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherGroups teacherGroups, int? groupId, int? techId)
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Teachers = await _db.Teachers.Where(x => !x.IsDeactive).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }

            TeacherGroups dbTeacherGroups = await _db.TeacherGroups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacherGroups == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbTeacherGroups);
            }
          
            dbTeacherGroups.Subject = teacherGroups.Subject;
            dbTeacherGroups.GroupId = (int)groupId;
            dbTeacherGroups.TeacherId = (int)techId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TeacherGroups dbTeacherGroups = await _db.TeacherGroups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacherGroups == null)
            {
                return BadRequest();
            }
            if (dbTeacherGroups.IsDeactive)
            {
                dbTeacherGroups.IsDeactive = false;
            }
            else
            {
                dbTeacherGroups.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
