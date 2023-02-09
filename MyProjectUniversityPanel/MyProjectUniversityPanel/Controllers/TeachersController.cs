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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            Designation designation = await _db.Designations.FirstOrDefaultAsync(x => x.Name == "Teacher");
            if (designation == null)
            {
                TempData["Message"] = "You have to create Designation first";
              
              
                //return View();
                
            }
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
        public async Task<IActionResult> Create(Teacher teacher, int? depId, int? genId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (depId == null || genId == null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                UserName = teacher.UserName,
                Image = teacher.Image
            };

            Designation designation = await _db.Designations.FirstOrDefaultAsync(x => x.Name == "Teacher");
            //if (designation==null)
            //{
                
            //}
            //if (designation==null)
            //{
            //    Designation designation1 = new Designation
            //    {
            //        Name="Teacher"
            //    };
            //}
            Staff staff = new Staff
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                Image = teacher.Image,
                File = teacher.Image,
                Birthday = DateTime.UtcNow.AddHours(4),
                GenderId = (int)genId,
                DesignationId = designation.Id,
                Education = "",
                Number = teacher.Number,
                Address = "",
                Salary = 0,
                JoiningDate = DateTime.UtcNow.AddHours(4),
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, "Admin1234");
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

                staff.Image = await teacher.Photo.SaveFileAsync(folder);
                staff.File = await teacher.Photo.SaveFileAsync(folder);

            }
            else
            {
                teacher.Image = "user.png";
                appUser.Image = "user.png";
                staff.Image = "user.png";
                staff.File = "user.png";
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

            teacher.DepartmentId = (int)depId;
            teacher.GenderId = (int)genId;
            teacher.JoiningDate = DateTime.UtcNow.AddHours(4);
            await _userManager.UpdateAsync(appUser);
            await _db.Teachers.AddAsync(teacher);
            await _db.Staff.AddAsync(staff);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> CreateDepartment()
        //{
        //    return RedirectToAction("CreateDepartment", "Teachers");

        //}
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
            Teacher dbTeacher = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IActionResult> Update(int? id, Teacher teacher, UpdateVM updateVM, int? depId, int? genId)
        {


            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();

            if (depId == null || genId == null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dbTeacher);
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
            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Email == user.Email);


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
            dbStaff.FullName = teacher.FullName;
            dbStaff.Email = teacher.Email;
            dbStaff.Number = teacher.Number;
            dbStaff.GenderId = (int)genId;
            dbTeacher.GenderId = (int)genId;
            dbTeacher.DepartmentId = (int)depId;
            dbTeacher.FullName = teacher.FullName;
            dbTeacher.UserName = teacher.UserName;
            dbTeacher.Email = teacher.Email;
            dbTeacher.Number = teacher.Number;
            dbTeacher.Degree = teacher.Degree;
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Teachers");
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
            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (dbTeacher.IsDeactive)
            {
                dbTeacher.IsDeactive = false;
                user.IsDeactive = false;
                dbStaff.IsDeactive = false;

            }
            else
            {
                dbTeacher.IsDeactive = true;
                user.IsDeactive = true;
                dbStaff.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher teacher = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
            {
                return BadRequest();
            }
            return View(teacher);
        }
        public async Task<IActionResult> SendEmail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Teacher dbTeacher = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(int? id, Email email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null)
            {
                return NotFound();
            }
            if (email.MessageSubject == null)
            {
                ModelState.AddModelError("MessageSubject", "The Subject field is required.");
                return View();
            }
            if (email.MessageBody == null)
            {
                ModelState.AddModelError("MessageBody", "The Message Body field is required.");
                return View();
            }
            Teacher dbTeacher = await _db.Teachers.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }




            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", dbTeacher.Email);
            message.Subject = email.MessageSubject;
            message.Body = email.MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                await client.SendMailAsync(message);

                TempData["Message"] = "Email has been sent";
            }
            catch (System.Exception ex)
            {
                TempData["Message"] = "Email was not sent " + ex.Message;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult SendEmailAll()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailAll(Email email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (email.MessageSubject == null)
            {
                ModelState.AddModelError("MessageSubject", "The Subject field is required.");
                return View();
            }
            if (email.MessageBody == null)
            {
                ModelState.AddModelError("MessageBody", "The Message Body field is required.");
                return View();
            }

            List<Teacher> teachers = await _db.Teachers.ToListAsync();
            foreach (Teacher item in teachers)
            {
                SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", item.Email);
                message.Subject = email.MessageSubject;
                message.Body = email.MessageBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                try
                {
                    await client.SendMailAsync(message);

                    TempData["Message"] = "Email has been sent";
                }
                catch (System.Exception ex)
                {
                    TempData["Message"] = "Email was not sent " + ex.Message;
                }
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
