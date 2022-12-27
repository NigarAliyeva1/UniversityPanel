using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyProjectUniversityPanel.Controllers
{

    [Authorize(Roles = "Student")]


    public class StudentHomeworksController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public StudentHomeworksController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            Student student = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            StudentGroup studentGroup = await _db.StudentGroups.Include(x => x.Student).FirstOrDefaultAsync(x => x.StudentId == student.Id);
            Group group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == studentGroup.GroupId);
            ViewBag.Homeworks = await _db.Homeworks.Include(x => x.Group).Include(x => x.AppUser).Where(x => x.GroupId == group.Id).ToListAsync();

            return View();
        }

        public async Task<IActionResult> DownloadFile(int? id)
        {
            Homework dbHomework = await _db.Homeworks.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHomework == null)
            {
                return BadRequest();
            }
            var memory = DownloadSinghFile(dbHomework.File, "wwwroot\\assets\\files");
            return File(memory.ToArray(), "text/plain", dbHomework.File);
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
        public async Task<IActionResult> UploadFile(int? id)
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


            return View(dbHomework);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(int? id, Homework homework)
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

            if (!ModelState.IsValid)
            {
                return View(dbHomework);
            }


            if (homework.UploadHomeFile != null)
            {
                if (homework.UploadHomeFile.IsOlder1MB())
                {
                    ModelState.AddModelError("UploadHomeFile", "Please choose Max 1mb file");
                    return View(dbHomework);
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "files");
                dbHomework.HomeFile = await homework.UploadHomeFile.SaveFileAsync(folder);
            }
            dbHomework.IsHomework = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
