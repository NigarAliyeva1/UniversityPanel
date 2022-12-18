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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class KassaController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public KassaController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            //List<Kassa> kassa = await _db.kassa.ToListAsync();
            //return View(kassa);
            return View();
        }
        //public async Task<IActionResult> Create()
        //{

        //    return View();

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Fees fee,int? income,int? outcome)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    fee.PayDate = DateTime.UtcNow.AddHours(4);
            
        //    //staff.Birthday = birthday;
        //    //staff.GenderId = (int)genId;
        //    //staff.JoiningDate = DateTime.UtcNow.AddHours(4);
        //    await _db.Fees.AddAsync(fee);
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}


        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Fees fees = await _db.Fees.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
        //    if (fees == null)
        //    {
        //        return BadRequest();
        //    }
        //    return View(fees);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeletePost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Fees fees = await _db.Fees.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == id);
        //    if (fees == null)
        //    {
        //        return BadRequest();
        //    }
        //    fees.IsDeactive = true;
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }
}
