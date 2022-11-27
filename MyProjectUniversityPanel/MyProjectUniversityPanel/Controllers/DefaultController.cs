using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
