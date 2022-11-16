using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{
   
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
