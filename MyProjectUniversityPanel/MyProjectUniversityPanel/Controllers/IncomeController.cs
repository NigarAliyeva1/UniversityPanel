using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class IncomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public IncomeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Income> incomes = await _db.Incomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).ToListAsync();
            return View(incomes);
        }
        public IActionResult Create()
        {
           
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Income income)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = appUser.FullName;
            kassa.Balance += income.Money;
            kassa.LastModifiedMoney = income.Money;
            kassa.LastModifiedFor = income.For;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            income.Date = DateTime.UtcNow.AddHours(4);
            income.AppUserId = appUser.Id;
            await _db.Incomes.AddAsync(income);
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

            Income dbIncome = await _db.Incomes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbIncome == null)
            {
                return BadRequest();
            }

            return View(dbIncome);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Income income, DateTime date)
        {

            if (id == null)
            {
                return NotFound();
            }

            Income dbIncome = await _db.Incomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbIncome == null)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(dbIncome);
            }

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = appUser.FullName;
            kassa.Balance -= dbIncome.Money;
            kassa.Balance += income.Money;
            kassa.LastModifiedMoney = income.Money;
            kassa.LastModifiedFor = income.For;
            kassa.LastModifiedTime = date;
            dbIncome.Date = date;
            dbIncome.AppUserId = appUser.Id;
            dbIncome.Money= income.Money;
            dbIncome.For= income.For;
           
         
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
            Income dbIncome = await _db.Incomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbIncome == null)
            {
                return BadRequest();
            }
            return View(dbIncome);
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
            Income dbIncome = await _db.Incomes.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
            if (dbIncome == null)
            {
                return BadRequest();
            }
            dbIncome.IsDeactive = true;
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.Balance -= dbIncome.Money;
            kassa.LastModifiedBy = appUser.FullName;
            kassa.LastModifiedFor = "Delete income -" + dbIncome.For;
            kassa.LastModifiedMoney = dbIncome.Money;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
