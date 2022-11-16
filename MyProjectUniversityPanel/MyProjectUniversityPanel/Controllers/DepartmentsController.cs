using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.DAL;
using University.Models;

namespace MyProjectUniversityPanel.Controllers
{

    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public DepartmentsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
           
            return View();
        }
       //public async Task<IActionResult> Index()
       // {
       //     List<Department> departments = await _db.Departments.ToListAsync();
       //     return View(departments);
       // }
       
    }
}
