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

namespace Fiorella.Areas.Admin.Controllers
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
        public async Task<IActionResult> Create(RegisterVM register,string createRole)
        {
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
            }
            else
            {
                appUser.Image = "user.png";
            }
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(appUser, createRole);
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Error");
                return View();
            }
            await _userManager.UpdateAsync(appUser);

            //if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Admin.ToString())
            //{
            //    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Admin.ToString());
            //}
            //else if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Teacher.ToString())
            //{
            //    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Teacher.ToString());
            //}
            //else if (register.RoleSelected != null && register.RoleSelected.Length > 0 && register.RoleSelected == Helper.Roles.Student.ToString())
            //{
            //    await _userManager.AddToRoleAsync(appUser, Helper.Roles.Student.ToString());
            //}
            //await _userManager.UpdateAsync(appUser);
            return RedirectToAction("Index");
        }




        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    AppUser user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }
        //    return View(user);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeletePost(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    AppUser user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }
        //    user.IsDeactive = true;
        //    await _userManager.UpdateAsync(user);
        //    return RedirectToAction("Index");
        //}




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
                RoleSelected = oldRole,
                RoleList = ListItems
            };
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
            return RedirectToAction("Index");
        }

    }
}
