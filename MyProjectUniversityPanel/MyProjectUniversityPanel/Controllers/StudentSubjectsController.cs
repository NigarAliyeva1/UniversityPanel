using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{
    public class StudentSubjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
