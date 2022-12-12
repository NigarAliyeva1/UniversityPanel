using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class TeachersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public TeachersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Teacher> teachers = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).ToListAsync();
            //ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ////int count = await _db.Departments.Where(x => !x.IsDeactive).CountAsync();
            ////ViewBag.Count = count;
            //ViewBag.Genders = await _db.Genders.ToListAsync();
            return View(teachers);
        }

        //public async Task<IActionResult> New()
        //{

        //    return View();

        //}
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher, int depId, int genId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                UserName = teacher.UserName,
                Image = teacher.Image
            };


            IdentityResult identityResult = await _userManager.CreateAsync(appUser, teacher.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            if (teacher.Photo != null)
            {
                if (!teacher.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View();
                }
                if (teacher.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                appUser.Image = await teacher.Photo.SaveFileAsync(folder);
                teacher.Image = await teacher.Photo.SaveFileAsync(folder);
            }
            else
            {
                teacher.Image = "user.png";
                appUser.Image = "user.png";
            }
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(appUser, Helper.Roles.Teacher.ToString());
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Error");
                return View();
            }
            bool isExist = await _db.Teachers.AnyAsync(x => x.UserName == teacher.UserName);
            if (isExist)
            {
                ModelState.AddModelError("UserName", "This teacher is already exist");
                return View();
            }
            teacher.DepartmentId = depId;
            teacher.GenderId = genId;
            teacher.JoiningDate = DateAndTime.Now;
            await _userManager.UpdateAsync(appUser);
            await _db.Teachers.AddAsync(teacher);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> CreateDepartment()
        //{
        //    return RedirectToAction("CreateDepartment", "Teachers");

        //}
        public async Task<IActionResult> Update(AppUser appUser,int? id)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            if (appUser == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,

            };
            return View(dbTeacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Teacher teacher, UpdateVM updateVM, AppUser appUser, int depId, int genId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
           

            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email

            };

            //if (!ModelState.IsValid)
            //{
            //    return View(dbTeacher);
            //}


            bool isExist1 = await _db.Teachers.AnyAsync(x => x.UserName == teacher.UserName && x.Id != id);
            if (isExist1)
            {
                ModelState.AddModelError("UserName", "This username is already exist");
                return View(dbTeacher);
            }
            if (teacher.Photo != null)
            {
                if (!teacher.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View(dbTeacher);
                }
                if (teacher.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View(dbTeacher);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                user.Image = await teacher.Photo.SaveFileAsync(folder);
                dbTeacher.Image = await teacher.Photo.SaveFileAsync(folder);
            }
            user.FullName = teacher.FullName;
            user.UserName = teacher.UserName;
            user.Email = teacher.Email;

            dbTeacher.GenderId = genId;
            dbTeacher.DepartmentId = depId;
            dbTeacher.FullName = teacher.FullName;
            dbTeacher.UserName = teacher.UserName;
            dbTeacher.Email = teacher.Email;
            dbTeacher.Number = teacher.Number;
            dbTeacher.Degree = teacher.Degree;
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index","Teachers");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
           
            if (dbTeacher.IsDeactive)
            {
                dbTeacher.IsDeactive = false;
                user.IsDeactive = false;

            }
            else
            {
                dbTeacher.IsDeactive = true;
                user.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ResetPassword(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int? id,Teacher teacher)
        {

            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
           

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, teacher.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            dbTeacher.Password = teacher.Password;
            dbTeacher.ConfirmPassword = teacher.ConfirmPassword;


            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
