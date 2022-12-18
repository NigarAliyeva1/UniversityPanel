using iTextSharp.text;
using MyProjectUniversityPanel.Models;
using System.Collections.Generic;

namespace MyProjectUniversityPanel.ViewModels
{
    public class HomeVM
    {
        public Kassa Kassa { get; set; }
        public Income Income { get; set; }
        public Outcome Outcome { get; set; }
        public List<Student> Students { get; set; }
    }
}
