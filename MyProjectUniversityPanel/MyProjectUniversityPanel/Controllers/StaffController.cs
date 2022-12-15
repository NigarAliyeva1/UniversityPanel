using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]


    public class StaffController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public StaffController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Staff> staff = await _db.Staff.Include(x => x.Gender).ToListAsync();
            return View(staff);
        }
        public async Task<IActionResult> Create()
        {
           
            ViewBag.Genders = await _db.Genders.ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff,DateTime birthday, int? genId)
        {
           
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (staff.Photo != null)
            {
                if (!staff.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View();
                }
                if (staff.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
              
                staff.Image = await staff.Photo.SaveFileAsync(folder);
            }
            else
            {
                staff.Image = "user.png";
            }

            if (staff.UploadFile != null)
            {
                if (staff.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
              
                staff.File = await staff.UploadFile.SaveFileAsync(folder);
            }
            
            staff.Birthday= birthday;
            staff.GenderId = (int)genId;
            staff.JoiningDate = DateTime.UtcNow.AddHours(4);
            await _db.Staff.AddAsync(staff);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Staff staff = await _db.Staff.Include(x => x.Gender).FirstOrDefaultAsync(x => x.Id == id);
            if (staff == null)
            {
                return BadRequest();
            }
            return View(staff);
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Staff staff = await _db.Staff.FirstOrDefaultAsync(x => x.Id == id);
            if (staff == null)
            {
                return BadRequest();
            }

            if (staff.IsDeactive)
            {
                staff.IsDeactive = false;
                

            }
            else
            {
                staff.IsDeactive = true;
               

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
         
            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
            {
                return BadRequest();
            }

            return View(dbStaff);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Staff staff, DateTime birthday, int? genId)
        {
        
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }


            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
            {
                return BadRequest();
            }

     

            if (!ModelState.IsValid)
            {
                return View(dbStaff);
            }


            if (staff.Photo != null)
            {
                if (!staff.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View(dbStaff);
                }
                if (staff.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View(dbStaff);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
              
                dbStaff.Image = await staff.Photo.SaveFileAsync(folder);
            }

            if (staff.UploadFile != null)
            {
                if (staff.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View(dbStaff);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");

                dbStaff.File = await staff.UploadFile.SaveFileAsync(folder);
            }

            dbStaff.GenderId = (int)genId;
            dbStaff.Birthday=birthday;
            dbStaff.FullName = staff.FullName;
            dbStaff.Email = staff.Email;
            dbStaff.Number = staff.Number;
            dbStaff.Address = staff.Address;
            dbStaff.Education = staff.Education;
            dbStaff.Designation = staff.Designation;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
