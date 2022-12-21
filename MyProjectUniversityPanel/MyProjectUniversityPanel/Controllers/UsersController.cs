using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Ocsp;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static iTextSharp.text.pdf.PdfDocument;
using System.Net.Mail;
using System.Net;

namespace MyProjectUniversityPanel.Controllers
{

    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;


        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();

            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    Image = user.Image
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);

        }


        public IActionResult Create()
        {
           
            List<SelectListItem> ListItems = new List<SelectListItem>();
            

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Teacher.ToString(),
                Text = Helper.Roles.Teacher.ToString()

            });

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Student.ToString(),
                Text = Helper.Roles.Student.ToString()

            });
            ListItems.Add(new SelectListItem()
            {
                Value=Helper.Roles.Admin.ToString(),
                Text=Helper.Roles.Admin.ToString()
            });

         
            RegisterVM registerVM = new RegisterVM();
            registerVM.RoleList = ListItems;
            return View(registerVM);
    
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM register,string createRole)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Teacher teacher = new Teacher
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Image = register.Image,
                Degree = "null",
                Number = "null",
                DepartmentId=1,
                GenderId=3,
                JoiningDate=DateTime.Now,
               
            };
            Student student = new Student
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Image = register.Image,
                Degree = "null",
                Number = "null",
                DepartmentId = 1,
                GenderId = 3,
                AdmissionDate = DateTime.Now,
            };
            AppUser appUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Image = register.Image
            };
            Designation designation = await _db.Designations.FirstOrDefaultAsync(x => x.Name == "Teacher");
            Staff staff = new Staff
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                Image = teacher.Image,
                File = teacher.Image,
                Birthday = DateTime.UtcNow.AddHours(4),
                GenderId = 3,
                DesignationId = designation.Id,
                Education = "",
                Number = teacher.Number,
                Address = "",
                Salary = 0,
                JoiningDate = DateTime.UtcNow.AddHours(4),
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            if (register.Photo != null)
            {
                if (!register.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View();
                }
                if (register.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }

                string folder = Path.Combine(_env.WebRootPath, "assets","images");
                appUser.Image = await register.Photo.SaveFileAsync(folder);
                teacher.Image = await register.Photo.SaveFileAsync(folder);
                student.Image = await register.Photo.SaveFileAsync(folder);
                staff.Image = await register.Photo.SaveFileAsync(folder);


            }
            else
            {
                teacher.Image = "user.png";
                student.Image = "user.png";
                appUser.Image = "user.png";
                staff.Image = "user.png";
            }
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(appUser, createRole);
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Error");
                return View();
            }
            if (createRole == Helper.Roles.Teacher.ToString())
            {
                await _db.Teachers.AddAsync(teacher);
                await _db.Staff.AddAsync(staff);
                
            }
            if (createRole == Helper.Roles.Student.ToString())
            {
                await _db.Students.AddAsync(student);
                
            }
            await _userManager.UpdateAsync(appUser);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.IsDeactive)
            {
                user.IsDeactive = false;
            }
            else
            {
                user.IsDeactive = true;

            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            Staff dbStaff = await _db.Staff.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (await _userManager.IsInRoleAsync(user, "Teacher"))
            {
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
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Update(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
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
            return View(dbUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, AppUser appUser)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
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
                return View(dbUpdateVM);
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x=>x.UserName== user.UserName);
            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x=>x.UserName== user.UserName);
            Staff dbStaff= await _db.Staff.FirstOrDefaultAsync(x => x.Email == user.Email);

            bool isExist = await _db.Users.AnyAsync(x => x.UserName == updateVM.UserName && x.Id != appUser.Id);
            if (isExist)
            {
                ModelState.AddModelError("", "Username is alrready exist");
                return View(dbUpdateVM);
            }

            if (appUser.Photo != null)
            {
                if (!appUser.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View(user);
                }
                if (appUser.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View(user);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                user.Image = await appUser.Photo.SaveFileAsync(folder);
            }
            user.FullName = updateVM.FullName;
            user.UserName = updateVM.UserName;
            user.Email = updateVM.Email;
            if (await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                dbTeacher.FullName = updateVM.FullName;
                dbTeacher.UserName = updateVM.UserName;
                dbTeacher.Email = updateVM.Email;
                dbStaff.Email = updateVM.Email;
                dbStaff.FullName = updateVM.FullName;
                await _db.SaveChangesAsync();
            }
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                dbStudent.FullName = updateVM.FullName;
                dbStudent.UserName = updateVM.UserName;
                dbStudent.Email = updateVM.Email;
                await _db.SaveChangesAsync();
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ResetPassword(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM)
        {

            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
           
          
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }

          
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Admin.ToString(),
                Text = Helper.Roles.Admin.ToString()

            });

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Teacher.ToString(),
                Text = Helper.Roles.Teacher.ToString()

            });

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Student.ToString(),
                Text = Helper.Roles.Student.ToString()

            });
           
            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRole = new ChangeRoleVM
            {
                Username = user.UserName,
                FullName=user.FullName,
                RoleSelected = oldRole,
                RoleList = ListItems
            };
            return View(changeRole);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, string newRole)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Admin.ToString(),
                Text = Helper.Roles.Admin.ToString()

            });

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Teacher.ToString(),
                Text = Helper.Roles.Teacher.ToString()

            });

            ListItems.Add(new SelectListItem()
            {
                Value = Helper.Roles.Student.ToString(),
                Text = Helper.Roles.Student.ToString()

            });

            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRole = new ChangeRoleVM
            {
                Username = user.UserName,
                FullName = user.FullName,
                RoleSelected = oldRole,
                RoleList = ListItems
            };
            if (!await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addIdentityResult.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View(changeRole);
                }
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (!removeIdentityResult.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View(changeRole);
                }
            }
            
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            DepartmentDetail dbDepartmentDetail = await _db.DepartmentDetails.Include(x=>x.Department).Include(x=>x.Teacher).FirstOrDefaultAsync(x => x.Teacher.UserName == user.UserName);
            if (dbDepartmentDetail.Teacher==dbTeacher)
            {
                ModelState.AddModelError("RoleSelected", "You can't change it");
                return View(changeRole);
            }
            else if (oldRole=="Teacher"&&dbTeacher!=null)
            {

                IdentityResult addIdentityResult1 = await _userManager.AddToRoleAsync(user, newRole);
                if (!addIdentityResult1.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View(changeRole);
                }
                IdentityResult removeIdentityResult1 = await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (!removeIdentityResult1.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View(changeRole);
                }
                _db.Teachers.Remove(dbTeacher);
                await _db.SaveChangesAsync();
            }
            if (newRole == "Teacher")
            {
                Teacher teacher = new Teacher
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Image = user.Image,
                    Degree = "null",
                    Number = "null",
                    DepartmentId = 1,
                    GenderId = 3,
                    JoiningDate = DateTime.UtcNow.AddHours(4),
                  
                };
                _db.Teachers.Add(teacher);
                await _db.SaveChangesAsync();
            }
            if (oldRole=="Student"&&dbStudent!=null)
            {
                _db.Students.Remove(dbStudent);
                await _db.SaveChangesAsync();
            }
            if (newRole == "Student")
            {
                Student student = new Student
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Image = user.Image,
                    Degree = "null",
                    Number = "null",
                    DepartmentId = 1,
                    GenderId = 3,
                    AdmissionDate = DateTime.UtcNow.AddHours(4),
                  
                };
                _db.Students.Add(student);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SendEmail(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(string id, Email email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null)
            {
                return NotFound();
            }
            if (email.MessageSubject==null)
            {
                ModelState.AddModelError("MessageSubject", "The Subject field is required.");
                return View();
            }
            if (email.MessageBody==null)
            {
                ModelState.AddModelError("MessageBody", "The Message Body field is required.");
                return View();
            }

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }


            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", user.Email);
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
            List<AppUser> users = await _userManager.Users.ToListAsync();
            foreach (AppUser item in users)
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
