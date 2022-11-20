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

namespace MyProjectUniversityPanel.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }


        int countLog = 0;
        public IActionResult Login()
        {
            ViewBag.Count = countLog;

            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

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
                ModelState.AddModelError("UserName", "This ccount has been blocked for a while");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("UserName", "Username or password is wrong!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        int count = 0;
        public async Task<IActionResult> Register(string returnUrl = null)
        {
          
            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            if (!(await _roleManager.RoleExistsAsync(Helper.Roles.SuperAdmin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.SuperAdmin.ToString() });
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString() });
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Teacher.ToString() });
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Student.ToString() });
            }
            if (count == 0)
            {
                count++;
                List<SelectListItem> ListItems = new List<SelectListItem>();

                ListItems.Add(new SelectListItem()
                {
                    Value = Helper.Roles.SuperAdmin.ToString(),
                    Text = Helper.Roles.SuperAdmin.ToString()

                });
                RegisterVM registerVM = new RegisterVM();
                registerVM.RoleList = ListItems;
                registerVM.ReturnUrl = returnUrl;
                
                return View(registerVM);
            }
            else
            {
                List<SelectListItem> ListItems = new List<SelectListItem>();

                ListItems.Add(new SelectListItem()
                {
                    Value = Helper.Roles.SuperAdmin.ToString(),
                    Text = Helper.Roles.SuperAdmin.ToString()

                });
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
                registerVM.ReturnUrl = returnUrl;
                return View(registerVM);
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register, string returnUrl = null)
        {
            //if (burada ancaq superadmin register ola bilecek)
            //{

            //}

            register.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Image = register.Image
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
                string folder = Path.Combine(_env.WebRootPath,"assets","images");
                appUser.Image = await appUser.Photo.SaveFileAsync(folder);
            }
            else
            {
                appUser.Image = "user.png";
            }
           
            await _signInManager.SignInAsync(appUser, true);
            if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.SuperAdmin.ToString())
            {
                await _userManager.AddToRoleAsync(appUser, Helper.Roles.SuperAdmin.ToString());
            }
            else if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Admin.ToString())
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
            return RedirectToAction("Index", "Home");
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
        //public async Task CreateRoles()
        //{
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.SuperAdmin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.SuperAdmin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Admin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Teacher.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Teacher.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Student.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Student.ToString() });
        //    }
        //}
    }
}
