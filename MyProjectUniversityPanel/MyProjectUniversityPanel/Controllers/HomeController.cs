using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class HomeController : Controller
    {
    
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
