using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "Teacher")]

    public class TeacherGroupsSubjectsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public TeacherGroupsSubjectsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            Teacher teacher = await _db.Teachers.Include(x => x.teacherGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            //TeacherGroups teacherGroups = await _db.TeacherGroups.Include(x => x.Teacher).FirstOrDefaultAsync(x => x.TeacherId == teacher.Id);
            ViewBag.Teachers = await _db.TeacherGroups.Include(x => x.Group).Include(x => x.Teacher).Where(x => x.TeacherId == teacher.Id).ToListAsync();

            return View();
        }
    }
}
