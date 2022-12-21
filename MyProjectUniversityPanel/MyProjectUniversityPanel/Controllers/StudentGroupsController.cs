using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "Student")]


    public class StudentGroupsController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public StudentGroupsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        
        public async Task<IActionResult> Index()
        {
            Student student = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            StudentGroup studentGroup = await _db.StudentGroups.Include(x => x.Student).FirstOrDefaultAsync(x => x.StudentId== student.Id);
            Group group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == studentGroup.GroupId);
            ViewBag.Students = await _db.StudentGroups.Include(x => x.Group).Include(x => x.Student).Where(x => x.GroupId == group.Id).ToListAsync();

            return View();
        }

    }
}
