using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class StaffController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
