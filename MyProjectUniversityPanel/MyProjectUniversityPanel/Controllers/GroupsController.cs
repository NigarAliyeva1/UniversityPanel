using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using static iTextSharp.tool.xml.html.HTML;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class GroupsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public GroupsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Group> groups = await _db.Groups.Include(x => x.Department).Include(x => x.StudentGroups).ThenInclude(x => x.Student).ToListAsync();
            return View(groups);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
           
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group, int? depId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
        
            if (!ModelState.IsValid)
            {
                return View();
            }
           
            if (depId == null)
            {
                return View();
            }
           
           
            group.DepartmentId = (int)depId;
            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
          
            if (id == null)
            {
                return NotFound();
            }
            Group dbGroup = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbGroup == null)
            {
                return BadRequest();
            }
            return View(dbGroup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Group group, int? depId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
           
            if (id == null)
            {
                return NotFound();
            }

            Group dbGroup = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbGroup == null)
            {
                return BadRequest();
            }
     
            if (!ModelState.IsValid)
            {
                return View(dbGroup);
            }
            bool isExist = await _db.Groups.AnyAsync(x => x.Number == group.Number && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Number", "This number is already exist");
                return View(dbGroup);
            }
            dbGroup.Number = group.Number;
            dbGroup.Capacity= group.Capacity;
            dbGroup.DepartmentId = (int)depId;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Group group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (group == null)
            {
                return BadRequest();
            }
            if (group.IsDeactive)
            {
                group.IsDeactive = false;
            }
            else
            {
                group.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        
        
    }
}
