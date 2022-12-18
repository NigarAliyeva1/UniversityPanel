using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SalaryController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public SalaryController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Salary> salaries = await _db.Salaries.Include(x => x.AppUser).Include(x => x.Staff).Where(x => !x.IsDeactive).ToListAsync();
            return View(salaries);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).ToListAsync();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salary salary, int? staffId)
        {
            ViewBag.Staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Staff staff=await _db.Staff.FirstOrDefaultAsync(x=>x.Id==(int)staffId);
            salary.Money = staff.Salary;
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = appUser.FullName;
            kassa.Balance -= salary.Money;
            kassa.LastModifiedMoney = salary.Money;
            kassa.LastModifiedFor = staff.FullName+" was paid";
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            Outcome outcome = new Outcome
            {
                Money=salary.Money,
                For= staff.FullName + " was paid",
                Date= DateTime.UtcNow.AddHours(4),
                AppUserId=appUser.Id,

            };
            salary.Date = DateTime.UtcNow.AddHours(4);
            salary.AppUserId = appUser.Id;
            
            await _db.Salaries.AddAsync(salary);
            await _db.Outcomes.AddAsync(outcome);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Salary dbSalary = await _db.Salaries.Include(x => x.AppUser).Include(x => x.Staff).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbSalary == null)
            {
                return BadRequest();
            }
            return View(dbSalary);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id, int? staffId)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            Salary dbSalary = await _db.Salaries.Include(x => x.AppUser).Include(x => x.Staff).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbSalary == null)
            {
                return BadRequest();
            }
            Staff staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == dbSalary.StaffId);
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Outcome outcome = await _db.Outcomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.For == staff.FullName + " was paid");
            outcome.IsDeactive=true;
            dbSalary.IsDeactive = true;
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.Balance += dbSalary.Money;
            kassa.LastModifiedBy = appUser.FullName;
            kassa.LastModifiedFor = "Delete payment of "+staff.FullName;
            kassa.LastModifiedMoney=dbSalary.Money;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
    }
}
