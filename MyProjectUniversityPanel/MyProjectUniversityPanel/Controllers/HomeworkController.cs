using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using static MyProjectUniversityPanel.Helpers.Helper;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Ocsp;

namespace MyProjectUniversityPanel.Controllers
{

    [Authorize(Roles = "Teacher")]

    public class HomeworkController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public HomeworkController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Homework> homeworks = await _db.Homeworks.Include(x => x.Group).Include(x=>x.AppUser).Where(x=>x.AppUser.UserName== User.Identity.Name).ToListAsync();

            return View(homeworks);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Homework homework, int? groupId,DateTime deadline)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (groupId == null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }

            if (homework.UploadFile != null)
            {
                if (homework.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("UploadFile", "Please choose Max 1mb image flie");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "files");

                homework.File = await homework.UploadFile.SaveFileAsync(folder);
            }
            homework.AppUserId = appUser.Id;
            homework.GroupId=(int)groupId;
            homework.Deadline=deadline;
            await _db.Homeworks.AddAsync(homework);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
   
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();

            if (id == null)
            {
                return NotFound();
            }
           
            Homework dbHomework = await _db.Homeworks.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHomework == null)
            {
                return BadRequest();
            }

          
            return View(dbHomework);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Homework homework, int? groupId, DateTime deadline)
        {

            ViewBag.Groups = await _db.Groups.Where(x => !x.IsDeactive).ToListAsync();

            if (groupId == null)
            {
                ModelState.AddModelError("", "Please choose one");
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }
            Homework dbHomework = await _db.Homeworks.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHomework == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dbHomework);
            }


            if (homework.UploadFile != null)
            {
                if (homework.UploadFile.IsOlder1MB())
                {
                    ModelState.AddModelError("UploadFile", "Please choose Max 1mb flie");
                    return View(dbHomework);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "files");

                dbHomework.File = await homework.UploadFile.SaveFileAsync(folder);
            }
            dbHomework.GroupId = (int)groupId;
         
            dbHomework.Deadline = deadline;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Homework dbHomework = await _db.Homeworks.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHomework == null)
            {
                return BadRequest();
            }
           
            if (dbHomework.IsDeactive)
            {
                dbHomework.IsDeactive = false;
               
            }
            else
            {
                dbHomework.IsDeactive = true;
               

            }
            await _db.SaveChangesAsync();
           
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DownloadFile(int? id)
        {
            Homework dbHomework = await _db.Homeworks.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHomework == null)
            {
                return BadRequest();
            }
            var memory = DownloadSinghFile(dbHomework.HomeFile, "wwwroot\\assets\\files");
            return File(memory.ToArray(), "text/plain", dbHomework.HomeFile);
        }
        private MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }



    }
}


