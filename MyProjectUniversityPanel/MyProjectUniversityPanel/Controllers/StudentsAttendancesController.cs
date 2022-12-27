using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{

    [Authorize(Roles = "SuperAdmin,Teacher")]

    public class StudentsAttendancesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public StudentsAttendancesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int? id)
        {
            List<StudentsAttendance> studentsAttendances = await _db.StudentsAttendances.Include(x => x.Student).ToListAsync();
            TeacherGroups teacherGroup = await _db.TeacherGroups.FirstOrDefaultAsync(x => x.Teacher.UserName == User.Identity.Name);
            ViewBag.StudentGroup = await _db.StudentGroups.Include(x => x.Group).FirstOrDefaultAsync(x => x.GroupId == id);
            return View(studentsAttendances);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int? id, StudentsAttendance studentsAttendance)
        {
            TeacherGroups teacherGroup = await _db.TeacherGroups.FirstOrDefaultAsync(x => x.Teacher.UserName == User.Identity.Name);
            ViewBag.StudentGroup = await _db.StudentGroups.Include(x => x.Group).FirstOrDefaultAsync(x => x.GroupId == id);

            StudentGroup studentGroup = await _db.StudentGroups.Include(x => x.Group).FirstOrDefaultAsync(x => x.GroupId == id);
            List<Student> students = await _db.Students.Where(x => x.Id == studentGroup.StudentId).ToListAsync();


            foreach (Student student in students)
            {
                if (!studentsAttendance.IsChecked)
                {
                    studentsAttendance.IsChecked = true;
                    studentsAttendance.StudentId = student.Id;
                    studentsAttendance.Date = DateTime.UtcNow.AddHours(4);
                    await _db.StudentsAttendances.AddAsync(studentsAttendance);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    studentsAttendance.IsChecked = false;
                    studentsAttendance.StudentId = student.Id;
                    studentsAttendance.Date = DateTime.UtcNow.AddHours(4);
                    await _db.StudentsAttendances.AddAsync(studentsAttendance);
                    await _db.SaveChangesAsync();
                }
            }



            return RedirectToAction("Index", "TeacherGroupsSubjects");
        }
        public async Task<IActionResult> Create()
        {

            List<Student> students = await _db.Students.Include(x => x.Gender).Include(x => x.Department).Include(x => x.StudentGroups).ThenInclude(x => x.Group).ToListAsync();
            ViewBag.Students = students;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int[] checkes)
        {
            List<Student> students = await _db.Students.Include(x => x.Gender).Include(x => x.Department).Include(x => x.StudentGroups).ThenInclude(x => x.Group).ToListAsync();
            ViewBag.Students = students;

            StudentsAttendance studentsAttendance = new StudentsAttendance();
            Student student = new Student();
            var chkDate = await _db.StudentsAttendances.Where(a => a.Date == DateTime.UtcNow.AddHours(4)).FirstOrDefaultAsync();
            if (chkDate == null)
            {
                foreach (var id in checkes)
                {
                    studentsAttendance.StudentId = id;
                    studentsAttendance.Date = DateTime.UtcNow.AddHours(4);
                    studentsAttendance.IsChecked=true;
                    await _db.StudentsAttendances.AddAsync(studentsAttendance);
                    await _db.SaveChangesAsync();

                    TempData["message"] = "Attendance has been marked";
                }
            }
            else
            {

                TempData["message"] = "Attendance has already been marked ";
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StudentsAttendance studentsAttendance = await _db.StudentsAttendances.FirstOrDefaultAsync(x => x.Id == id);
            if (studentsAttendance == null)
            {
                return BadRequest();
            }
            if (studentsAttendance.IsChecked)
            {
                studentsAttendance.IsChecked = false;
            }
            else
            {
                studentsAttendance.IsChecked = true;

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }

}
