using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static iTextSharp.tool.xml.html.HTML;

namespace MyProjectUniversityPanel.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
        public bool IsDeactive { get; set; }
        
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public List<Kassa> Kassas { get; set; }
        public List<Income> Incomes { get; set; }
        public List<Outcome> Outcomes { get; set; }
        public List<Salary> Salaries { get; set; }
        public List<Homework> Homeworks { get; set; }
        public List<StudentGrades> StudentGrades { get; set; }




    }
}
