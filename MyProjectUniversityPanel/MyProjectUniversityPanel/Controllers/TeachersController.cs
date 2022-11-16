using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{

    public class TeachersController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
