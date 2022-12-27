using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Helpers;
using MyProjectUniversityPanel.Models;
using MyProjectUniversityPanel.ViewModels;
using System;
using System.Collections.Generic;
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
        //Bu hisse qakdi tamamlaya bilmedim
        public async Task<IActionResult> Index()
        {
            Teacher teacher = await _db.Teachers.Include(x => x.teacherGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            ViewBag.Teachers = await _db.TeacherGroups.Include(x => x.Group).Include(x => x.Teacher).Where(x => x.TeacherId == teacher.Id).ToListAsync();

            return View();
        }
        public async Task<IActionResult> Students(int? id)
        {

            Group group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Students = await _db.StudentGroups.Include(x => x.Group).Include(x => x.Student).Where(x => x.GroupId == id).ToListAsync();
            List<StudentGroup> studentGroups = await _db.StudentGroups.Include(x => x.Group).Include(x => x.Student).Where(x => x.GroupId == id).ToListAsync();
            ViewBag.Group = group.Number;
           
            //ViewBag.Grades= await _db.StudentGrades.Include(x => x.Student).Where(x => x.StudentId == studentGroups).ToListAsync();
            return View();
        }
        public IActionResult Grades()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Grades(int? id,StudentGrades studentGrade)
        {

            if (id == null)
            {
                return NotFound();
            }

            Student dbStudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStudent == null)
            {
                return BadRequest();

            }
            Teacher teacher = await _db.Teachers.Include(x => x.teacherGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            StudentGrades newStudentGrade = new StudentGrades
            {
                StudentId = dbStudent.Id,
                TeacherId = teacher.Id,
                AppUserId = appUser.Id,

            };
            await _db.StudentGrades.AddAsync(newStudentGrade);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        //public async Task<IActionResult> StudentsAttendances(string number)
        //{

        //    TeacherGroups teacherGroup = await _db.TeacherGroups.Include(x => x.Teacher).ThenInclude(x => x.teacherGroups).Include(x => x.Group).FirstOrDefaultAsync(x => x.Teacher.UserName == User.Identity.Name);

        //    ViewBag.StudentGroups = await _db.StudentGroups.Include(x => x.Group).ThenInclude(x => x.StudentGroups).Include(x => x.Student).Where(x => x.Group.Number == number).ToListAsync();

        //    List<StudentsAttendance> studentsAttendances = await _db.StudentsAttendances.Include(x => x.Student).ThenInclude(x => x.StudentsAttendances).Include(x => x.Group).Where(x => x.Group.Number == number).ToListAsync();

        //    foreach (StudentGroup studentGroup in ViewBag.StudentGroups)
        //    {
        //        var chkDate = await _db.StudentsAttendances.Where(a => a.Date.Day == DateTime.UtcNow.AddHours(4).Day).FirstOrDefaultAsync();
        //        var chkStudent = await _db.StudentsAttendances.Include(x => x.Student).Where(a => a.StudentId == studentGroup.StudentId).Where(a => a.Date.Day == DateTime.UtcNow.AddHours(4).Day).FirstOrDefaultAsync();
        //        if (chkDate == null || chkStudent == null)
        //        {
        //            StudentsAttendance studentsAttendance = new StudentsAttendance
        //            {
        //                IsChecked = false,
        //                StudentId = studentGroup.StudentId,
        //                Date = DateTime.UtcNow.AddHours(4),
        //                GroupId = studentGroup.GroupId,
        //            };

        //            await _db.StudentsAttendances.AddAsync(studentsAttendance);
        //            await _db.SaveChangesAsync();
        //        }

        //    }
        //    return View(studentsAttendances);
        //}

        //public async Task<IActionResult> Activity(string number)
        //{
        //    //    if (number == null)
        //    //    {
        //    //        return NotFound();
        //    //    }


        //    StudentsAttendance dbStudentAttendance = await _db.StudentsAttendances.FirstOrDefaultAsync(x => x.Group.Number == number);

        //    if (dbStudentAttendance == null)
        //    {
        //        return BadRequest();
        //    }


        //    if (dbStudentAttendance.IsChecked)
        //    {
        //        dbStudentAttendance.IsChecked = false;

        //        await _db.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        dbStudentAttendance.IsChecked = true;

        //        await _db.SaveChangesAsync();
        //    }


        //    return RedirectToAction("TeacherGroupsSubjects", "StudentsAttendances", "Number");



        //}
    }
}

