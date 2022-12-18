using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MyProjectUniversityPanel.DAL;
using MyProjectUniversityPanel.Models;
using System.Linq;
using System.Threading.Tasks;

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
