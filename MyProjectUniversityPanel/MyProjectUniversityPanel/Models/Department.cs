using System.Collections.Generic;

namespace MyProjectUniversityPanel.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public bool IsDeactive { get; set; }
        public List<Teacher> Teachers { get; set; }


    }
}
