using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static MyProjectUniversityPanel.Helpers.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OutcomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public OutcomeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Outcome> outcomes = await _db.Outcomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).ToListAsync();
            return View(outcomes);
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Outcome outcome)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = appUser.FullName;
            kassa.Balance -= outcome.Money;
            kassa.LastModifiedMoney = outcome.Money;
            kassa.LastModifiedFor = outcome.For;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            outcome.Date = DateTime.UtcNow.AddHours(4);
            outcome.AppUserId = appUser.Id;
            await _db.Outcomes.AddAsync(outcome);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Update(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            Outcome dbOutcome = await _db.Outcomes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbOutcome == null)
            {
                return BadRequest();
            }
            Staff staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => dbOutcome.For.Contains(x.FullName));
            if (staff != null)
            {
                ViewBag.Staff = staff;
            }
           
            return View(dbOutcome);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Outcome outcome, DateTime date)
        {
            if (id == null)
            {
                return NotFound();
            }


            Outcome dbOutcome = await _db.Outcomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbOutcome == null)
            {
                return BadRequest();
            }
        
            if (!ModelState.IsValid)
            {
                return View(dbOutcome);
            }
            Staff staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x=>dbOutcome.For.Contains(x.FullName));
            if (staff!=null)
            {
                Salary dbSalary = await _db.Salaries.FirstOrDefaultAsync(x => x.StaffId == staff.Id);
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
                kassa.LastModifiedBy = appUser.FullName;
                kassa.Balance += dbOutcome.Money;
                kassa.Balance -= outcome.Money;
                kassa.LastModifiedMoney = outcome.Money;
                kassa.LastModifiedFor = outcome.For;
                kassa.LastModifiedTime = date;
                dbOutcome.AppUserId = appUser.Id;
                dbOutcome.Money = outcome.Money;
                dbSalary.Money = outcome.Money;
                dbOutcome.Date = dbOutcome.Date;
                dbOutcome.For = dbOutcome.For;

            }
            else
            {
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
                kassa.LastModifiedBy = appUser.FullName;
                kassa.Balance += dbOutcome.Money;
                kassa.Balance -= outcome.Money;
                kassa.LastModifiedMoney = outcome.Money;
                kassa.LastModifiedFor = outcome.For;
                kassa.LastModifiedTime = date;
                dbOutcome.AppUserId = appUser.Id;
                dbOutcome.Money = outcome.Money;
                dbOutcome.Date = date;
                dbOutcome.For = outcome.For;
            }
            
           
         


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

            Outcome dbOutcome = await _db.Outcomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbOutcome == null)
            {
                return BadRequest();
            }
            return View(dbOutcome);
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

            Outcome dbOutcome = await _db.Outcomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbOutcome == null)
            {
                return BadRequest();
            }
            dbOutcome.IsDeactive = true;
            Staff staff = await _db.Staff.Include(x => x.Designation).Include(x => x.Gender).Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => dbOutcome.For.Contains(x.FullName));
            if (staff != null)
            {
                Salary dbSalary = await _db.Salaries.FirstOrDefaultAsync(x => x.StaffId == staff.Id);
                dbSalary.IsDeactive = true;
            }

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.Balance += dbOutcome.Money;
            kassa.LastModifiedBy = appUser.FullName;
            kassa.LastModifiedFor = " Delete outcome - " + dbOutcome.For;
            kassa.LastModifiedMoney = dbOutcome.Money;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
