using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
using System.Collections;
using System.Net.Mime;

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
            List<Staff> staff = await _db.Staff.Include(x => x.Gender).Include(x => x.Designation).ToListAsync();
            return View(staff);
        }
        public async Task<IActionResult> Create()
        {

            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Designations = await _db.Designations.Where(x => !x.IsDeactive).ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff, DateTime birthday, int? genId, int? desId)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Designations = await _db.Designations.Where(x => !x.IsDeactive).ToListAsync();

            Teacher teacher = new Teacher
            {
                FullName = staff.FullName,
                Email = staff.Email,
                UserName = staff.Email,
                Image = staff.Image,
                Degree = "null",
                Number = staff.Number,
                DepartmentId = 1,
                GenderId = (int)genId,
                JoiningDate = DateTime.Now,
                
            };

            Designation designation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == (int)desId);
            AppUser appUser = new AppUser
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                UserName = teacher.UserName,
                Image = teacher.Image
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
                teacher.Image = await staff.Photo.SaveFileAsync(folder);
                appUser.Image = await staff.Photo.SaveFileAsync(folder);
            }
            else
            {
                staff.Image = "user.png";
                teacher.Image = "user.png";
                appUser.Image = "user.png";
            }

            if (staff.UploadFile != null)
            {
                if (staff.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("UploadFile", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");

                staff.File = await staff.UploadFile.SaveFileAsync(folder);
            }

            staff.Birthday = birthday;
            staff.GenderId = (int)genId;
            staff.DesignationId = (int)desId;
            staff.JoiningDate = DateTime.UtcNow.AddHours(4);
            await _db.Staff.AddAsync(staff);
            if (designation.Name == "Teacher")
            {
                await _db.Teachers.AddAsync(teacher);
                await _userManager.UpdateAsync(appUser);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Staff staff = await _db.Staff.Include(x => x.Gender).Include(x => x.Designation).FirstOrDefaultAsync(x => x.Id == id);
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
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Email == staff.Email);
            if (dbTeacher == null)
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            if (staff.IsDeactive)
            {
                staff.IsDeactive = false;
                dbTeacher.IsDeactive = false;
                user.IsDeactive = false;

            }
            else
            {
                staff.IsDeactive = true;
                dbTeacher.IsDeactive = true;
                user.IsDeactive = true;

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {

            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Designations = await _db.Designations.Where(x => !x.IsDeactive).ToListAsync();

            if (id == null)
            {
                return NotFound();
            }

            Staff dbStaff = await _db.Staff.Include(x => x.Gender).Include(x => x.Designation).FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
            {
                return BadRequest();
            }

            return View(dbStaff);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Staff staff, DateTime birthday, int? genId, int? desId)
        {

            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Designations = await _db.Designations.Where(x => !x.IsDeactive).ToListAsync();

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

            if (staff.Address==null)
            {
                ModelState.AddModelError("Address", "Please write your address");
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
                //user.Image = await staff.Photo.SaveFileAsync(folder);
                //dbTeacher.Image = await staff.Photo.SaveFileAsync(folder);
            }

            if (staff.UploadFile != null)
            {
                if (staff.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("UploadFile", "Please choose Max 1mb flie");
                    return View(dbStaff);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");

                dbStaff.File = await staff.UploadFile.SaveFileAsync(folder);
            }
        
            //if (await _userManager.IsInRoleAsync(user, "Teacher"))
            //{
               

            //}

            dbStaff.GenderId = (int)genId;
            dbStaff.DesignationId = (int)desId;
            dbStaff.Birthday = birthday;
            dbStaff.FullName = staff.FullName;
            dbStaff.Email = staff.Email;
            dbStaff.Number = staff.Number;
            dbStaff.Address = staff.Address;
            dbStaff.Education = staff.Education;
            dbStaff.Salary = staff.Salary;
            Designation designation = await _db.Designations.Where(x => !x.IsDeactive).FirstOrDefaultAsync(x => x.Id == dbStaff.DesignationId);
            if (designation != null)
            {
                Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Email == dbStaff.Email);
                AppUser user = await _userManager.FindByNameAsync(dbTeacher.UserName);
                user.FullName = staff.FullName;
                user.Email = staff.Email;
                user.Image = staff.Image;
                dbTeacher.GenderId = (int)genId;
                dbTeacher.FullName = staff.FullName;
                dbTeacher.Email = staff.Email;
                dbTeacher.Number = staff.Number;
                dbTeacher.Image= dbStaff.Image;
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendEmail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Staff dbStaff = await _db.Staff.Include(x => x.Gender).Include(x => x.Designation).FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
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

            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
            {
                return BadRequest();
            }

    
            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", dbStaff.Email);
            message.Subject =email.MessageSubject;
            message.Body = email.MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
           
            try
            {
                await client.SendMailAsync(message);

                TempData["Message"] = "Email has been sent";
            }
            catch(System.Exception ex)
            {
                TempData["Message"] = "Email was not sent "+ex.Message;
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
        public async Task<IActionResult> SendEmailAll( Email email)
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

            List<Staff> staff = await _db.Staff.ToListAsync();
            foreach (Staff item in staff)
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
