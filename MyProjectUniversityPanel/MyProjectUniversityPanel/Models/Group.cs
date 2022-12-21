using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static iTextSharp.tool.xml.html.HTML;

namespace MyProjectUniversityPanel.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int Capacity { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        //public List<Group> ChildrenStudents { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        public List<TeacherGroups> teacherGroups { get; set; }

        public bool IsDeactive { get; set; }
    }
}
