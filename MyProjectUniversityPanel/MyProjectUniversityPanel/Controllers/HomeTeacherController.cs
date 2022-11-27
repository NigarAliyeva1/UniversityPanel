using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "Teacher")]

    public class HomeTeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
