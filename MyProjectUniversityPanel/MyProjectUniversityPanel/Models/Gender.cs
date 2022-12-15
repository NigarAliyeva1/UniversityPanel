using System.Collections.Generic;

namespace MyProjectUniversityPanel.Models
{
    public class Gender
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Staff> Staff { get; set; }


    }
}
