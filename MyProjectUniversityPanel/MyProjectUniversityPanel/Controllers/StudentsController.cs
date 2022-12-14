using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]


    public class StudentsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public StudentsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Student> students = await _db.Students.Include(x => x.Gender).Include(x => x.Department).ToListAsync();
            //ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ////int count = await _db.Departments.Where(x => !x.IsDeactive).CountAsync();
            ////ViewBag.Count = count;
            //ViewBag.Genders = await _db.Genders.ToListAsync();
            return View(students);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, int? depId, int? genId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser
            {
                FullName = student.FullName,
                Email = student.Email,
                UserName = student.UserName,
                Image = student.Image
            };


            IdentityResult identityResult = await _userManager.CreateAsync(appUser, student.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            if (student.Photo != null)
            {
                if (!student.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View();
                }
                if (student.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                appUser.Image = await student.Photo.SaveFileAsync(folder);
                student.Image = await student.Photo.SaveFileAsync(folder);
            }
            else
            {
                student.Image = "user.png";
                appUser.Image = "user.png";
            }
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(appUser, Helper.Roles.Student.ToString());
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Error");
                return View();
            }
            bool isExist = await _db.Students.AnyAsync(x => x.UserName == student.UserName);
            if (isExist)
            {
                ModelState.AddModelError("UserName", "This username is already exist");
                return View();
            }
            student.DepartmentId = (int)depId;
            student.GenderId = (int)genId;
            student.AdmissionDate = DateAndTime.Now;
            await _userManager.UpdateAsync(appUser);
            await _db.Students.AddAsync(student);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(AppUser appUser, int? id)
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
            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbStudent.UserName);
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
            return View(dbStudent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Student student, UpdateVM updateVM,int? depId, int? genId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }


            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbStudent.UserName);
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
            //    return View(dbStudent);
            //}


            bool isExist1 = await _db.Students.AnyAsync(x => x.UserName == student.UserName && x.Id != id);
            if (isExist1)
            {
                ModelState.AddModelError("UserName", "This username is already exist");
                return View(dbStudent);
            }
            if (student.Photo != null)
            {
                if (!student.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View(dbStudent);
                }
                if (student.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View(dbStudent);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                user.Image = await student.Photo.SaveFileAsync(folder);
                dbStudent.Image = await student.Photo.SaveFileAsync(folder);
            }
            user.FullName = student.FullName;
            user.UserName = student.UserName;
            user.Email = student.Email;
                
            dbStudent.GenderId = (int)genId;
            dbStudent.DepartmentId = (int)depId;
            dbStudent.FullName = student.FullName;
            dbStudent.UserName = student.UserName;
            dbStudent.Email = student.Email;
            dbStudent.Number = student.Number;
            dbStudent.Degree = student.Degree;
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbStudent.UserName);
            if (user == null)
            {
                return BadRequest();
            }

            if (dbStudent.IsDeactive)
            {
                dbStudent.IsDeactive = false;
                user.IsDeactive = false;

            }
            else
            {
                dbStudent.IsDeactive = true;
                user.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}














