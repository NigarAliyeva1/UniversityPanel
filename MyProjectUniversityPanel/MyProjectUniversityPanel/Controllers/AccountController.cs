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
            _userManager=userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
            _env=env;
        }
        //public IActionResult Login()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return View("Error");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginVM loginVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    AppUser user=await _userManager.FindByNameAsync(loginVM.UserName);
        //    if (user==null)
        //    {
        //        ModelState.AddModelError("", "Username or password is wrong!");
        //        return View();
        //    }
        //    if (user.IsDeactive)
        //    {
        //        ModelState.AddModelError("UserName", "This ccount has been blocked");
        //        return View();
        //    }
        //    Microsoft.AspNetCore.Identity.SignInResult signInResult=await _signInManager.PasswordSignInAsync(user,loginVM.Password,true,true);
        //    if (signInResult.IsLockedOut)
        //    {
        //        ModelState.AddModelError("UserName", "This ccount has been blocked for a while");
        //        return View();
        //    }
        //    if (!signInResult.Succeeded)
        //    {
        //        ModelState.AddModelError("UserName", "Username or password is wrong!");
        //        return View();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterVM register)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    AppUser appUser = new AppUser
        //    {
        //        FullName = register.FullName,
        //        Email = register.Email,
        //        UserName = register.UserName,
        //        Image = register.Image
        //    };
        //    IdentityResult identityResult = await _userManager.CreateAsync(appUser, register.Password);
        //    if (!identityResult.Succeeded)
        //    {
        //        foreach (IdentityError error in identityResult.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //        return View();

        //    }
        //    if (!appUser.Photo.IsImage())
        //    {
        //        ModelState.AddModelError("Photo", "Please choose the image flie");
        //        return View();
        //    }
        //    if (appUser.Photo.IsOlder1MB())
        //    {
        //        ModelState.AddModelError("Photo", "Please choose Max 1mb image flie");
        //        return View();
        //    }
        //    string folder = Path.Combine(_env.WebRootPath, "img");
        //    appUser.Image = await appUser.Photo.SaveFileAsync(folder);

        //    await _signInManager.SignInAsync(appUser, true);
        //    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Member.ToString());

        //    return RedirectToAction("Index", "Home");
        //}
        //public async Task<IActionResult> Logout()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return View("Error");
        //    }
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}
        //public async Task CreateRoles()
        //{
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
