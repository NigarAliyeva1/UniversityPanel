using System.Collections.Generic;

namespace MyProjectUniversityPanel.Models
{
    public class Designation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Staff> Staff { get; set; }
        public bool IsDeactive { get; set; }


    }
}
