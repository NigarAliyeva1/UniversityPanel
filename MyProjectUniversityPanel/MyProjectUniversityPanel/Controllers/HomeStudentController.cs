using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "Student")]

    public class HomeStudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
