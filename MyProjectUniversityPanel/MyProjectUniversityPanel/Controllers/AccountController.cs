using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MyProjectUniversityPanel.DAL;
using System.Security.Principal;

namespace MyProjectUniversityPanel.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public AccountController(AppDbContext db, UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }

     
        public async Task<IActionResult> Login()
        {
            
            HasSuperAdmin hasAdmin = await _db.HasSuperAdmins.FirstOrDefaultAsync();
            if (hasAdmin.HasSuperadmin)
            {
                ViewBag.IsExistAdmin = true;
            }
            else
            {
                ViewBag.IsExistAdmin = false;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            HasSuperAdmin hasAdmin = await _db.HasSuperAdmins.FirstOrDefaultAsync();
            if (hasAdmin.HasSuperadmin)
            {
                ViewBag.IsExistAdmin = true;
            }
            else
            {
                ViewBag.IsExistAdmin = false;
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong!");
                return View();
            }
            if (user.IsDeactive)
            {
                ModelState.AddModelError("UserName", "This ccount has been blocked");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("UserName", "This account has been blocked for a while");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("UserName", "Username or password is wrong!");
                return View();
            }
            
            
            return RedirectToAction("Index", "Default");
        }
        public async Task<IActionResult> CreateAdmin()
        {

            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            await CreateRoles();
            AppUser appUser = await _userManager.FindByNameAsync("SuperAdmin");
            if (appUser != null)
            {
                return NotFound();
            }
            AppUser newUser = new AppUser
            {
                FullName = "SuperAdmin",
                UserName = "SuperAdmin",
                Email = "admin@admin.com",
                Image="user.png"
            };
            await _userManager.CreateAsync(newUser, "Admin1234");
            await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
         
            HasSuperAdmin hasSuperAdmin = await _db.HasSuperAdmins.FirstOrDefaultAsync();
            hasSuperAdmin.HasSuperadmin = true;

            _db.Entry(hasSuperAdmin).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            ViewBag.IsExistAdmin = true;

            return RedirectToAction("Login");
        }
        public async Task CreateRoles()
        {
            if (!(await _roleManager.RoleExistsAsync(Helper.Roles.SuperAdmin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.SuperAdmin.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Teacher.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Teacher.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Student.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Student.ToString() });
            }
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
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
            RegisterVM registerVM = new RegisterVM();
            registerVM.RoleList = ListItems;

            return View(registerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (register.RoleSelected == null)
            {
                ModelState.AddModelError("RoleSelected", "Please select a role");
                return View();
            }

            AppUser appUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Image = register.Image
            };
            if (appUser.Photo != null)
            {
                if (!appUser.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the image flie");
                    return View();
                }
                if (appUser.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
                    return View();
                }
                //ModelState.AddModelError("Photo", "Please choose an image");
                //return View();
                string folder = Path.Combine(_env.WebRootPath, "assets", "images");
                appUser.Image = await appUser.Photo.SaveFileAsync(folder);
            }
            else
            {
                appUser.Image = "user.png";
            }
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, register.Password);
            if (identityResult.Succeeded)
            {
                if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Admin.ToString())
                {
                    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Admin.ToString());
                }
                else if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Teacher.ToString())
                {
                    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Teacher.ToString());
                }
                else if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Student.ToString())
                {
                    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Student.ToString());
                }
                await _signInManager.SignInAsync(appUser, true);
                return RedirectToAction("Login");
            }
            else
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
           
            if (!User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    


    }
}
