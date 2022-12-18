using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static MyProjectUniversityPanel.Helpers.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MyProjectUniversityPanel.Controllers
{

    [Authorize(Roles = "Admin")]

    public class HomeAdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public HomeAdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Date = new DateTime(0001, 01, 01);
            ViewBag.Date2 = new DateTime(0001, 01, 01);
            ViewBag.AllIncome = 0;
            ViewBag.AllOutcome = 0;

            List<Income> incomes = await _db.Incomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).ToListAsync();
            List<Outcome> outcomes = await _db.Outcomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).ToListAsync();
            Income income = await _db.Incomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).OrderBy(x => x.Id).LastOrDefaultAsync();
            Outcome outcome = await _db.Outcomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).OrderBy(x => x.Id).LastOrDefaultAsync();
            HomeVM homeVM = new HomeVM
            {
                Kassa = await _db.Kassas.Include(x => x.AppUser).Where(x => !x.IsDeactive).OrderBy(x => x.Id).LastOrDefaultAsync(),
                Income = await _db.Incomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).OrderBy(x => x.Id).LastOrDefaultAsync(),
                Outcome = await _db.Outcomes.Include(x => x.AppUser).Where(x => !x.IsDeactive).OrderBy(x => x.Id).LastOrDefaultAsync(),

            };
            foreach (var item in incomes)
            {
                ViewBag.AllIncome += item.Money;
            }
            foreach (var item in outcomes)
            {
                ViewBag.AllOutcome += item.Money;
            }
            DateTime nowDate = DateTime.UtcNow.AddHours(4);
            if (nowDate.ToString("dd/MM/yyyy") == income.Date.ToString("dd/MM/yyyy"))
            {
                ViewBag.Date = "Today";
            }
            else if (nowDate.AddDays(-1).ToString("dd/MM/yyyy") == income.Date.ToString("dd/MM/yyyy"))
            {
                ViewBag.Date = "Yesterday";
            }
            else if ((nowDate - income.Date).TotalDays <= 7)
            {
                ViewBag.Date = income.Date.DayOfWeek.ToString();
            }
            else if ((nowDate - income.Date).TotalDays <= 14)
            {
                ViewBag.Date = "Last Week";
            }
            else if ((nowDate - income.Date).TotalDays <= 21)
            {
                ViewBag.Date = "Two Weeks Ago";
            }
            else if ((nowDate - income.Date).TotalDays <= 28)
            {
                ViewBag.Date = "Three Weeks Ago";
            }
            else if ((nowDate - income.Date).TotalDays <= 30)
            {
                ViewBag.Date = "Last Month";
            }
            else
            {
                ViewBag.Date2 = "Older";
            }

            if (nowDate.ToString("dd/MM/yyyy") == outcome.Date.ToString("dd/MM/yyyy"))
            {
                ViewBag.Date2 = "Today";
            }
            else if (nowDate.AddDays(-1).ToString("dd/MM/yyyy") == outcome.Date.ToString("dd/MM/yyyy"))
            {
                ViewBag.Date2 = "Yesterday";
            }
            else if ((nowDate - outcome.Date).TotalDays <= 7)
            {
                ViewBag.Date2 = outcome.Date.DayOfWeek.ToString();
            }
            else if ((nowDate - outcome.Date).TotalDays <= 14)
            {
                ViewBag.Date2 = "Last Week";
            }
            else if ((nowDate - outcome.Date).TotalDays <= 21)
            {
                ViewBag.Date2 = "Two Weeks Ago";
            }
            else if ((nowDate - outcome.Date).TotalDays <= 28)
            {
                ViewBag.Date2 = "Three Weeks Ago";
            }
            else if ((nowDate - outcome.Date).TotalDays <= 30)
            {
                ViewBag.Date2 = "Last Month";
            }
            else
            {
                ViewBag.Date2 = "Older";
            }
            return View(homeVM);
        }
    }
}
