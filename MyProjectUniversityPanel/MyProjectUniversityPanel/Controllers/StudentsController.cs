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
using System.Net.Mail;
using System.Net;
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

            List<Student> students = await _db.Students.Include(x => x.Gender).Include(x => x.Department).Include(x => x.StudentGroups).ThenInclude(x=>x.Group).ToListAsync();

            return View(students);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();


            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, int? depId, int? genId,int? groupId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            if (depId == null||genId == null||groupId==null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }
           
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


            IdentityResult identityResult = await _userManager.CreateAsync(appUser, "Admin1234");
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
            //List<StudentGroup> studentGroups = new List<StudentGroup>();
            //if (groupId != null)
            //{
            //    StudentGroup studentGroup = new StudentGroup
            //    {
            //        GroupId = (int)groupId,

            //    };
            //    studentGroups.Add(studentGroup);
            //}
            //student.StudentGroups = studentGroups;

            List<StudentGroup> studentGroups = new List<StudentGroup>();
            StudentGroup studentGroup = new StudentGroup
            {
                GroupId = (int)groupId,

            };
            studentGroup.GroupId = (int)groupId;
            studentGroups.Add(studentGroup);
            student.StudentGroups = studentGroups;

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
            Student dbStudent = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
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
        public async Task<IActionResult> Update(int? id, Student student, UpdateVM updateVM,int? depId, int? genId, int? groupId)
        {
            ViewBag.Departments = await _db.Departments.Where(x => !x.IsDeactive).ToListAsync();
            ViewBag.Genders = await _db.Genders.ToListAsync();
           
            if (depId == null || genId == null || groupId == null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }


            Student dbStudent = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
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

            if (!ModelState.IsValid)
            {
                return View(dbStudent);
            }


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
            List<StudentGroup> studentGroups = new List<StudentGroup>();
            StudentGroup studentGroup = new StudentGroup
            {
                GroupId = (int)groupId,

            };
            studentGroup.GroupId = (int)groupId;
            studentGroups.Add(studentGroup);

            dbStudent.StudentGroups = studentGroups;


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
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student student = await _db.Students.Include(x => x.Gender).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return BadRequest();
            }
            return View(student);
        }
        public async Task<IActionResult> SendEmail(int? id)
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
            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();
            }




            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", dbStudent.Email);
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

            List<Student> students = await _db.Students.ToListAsync();
            foreach (Student item in students)
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














