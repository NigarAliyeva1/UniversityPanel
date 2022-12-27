using MyProjectUniversityPanel.Models;
using System.Collections.Generic;

namespace MyProjectUniversityPanel.ViewModels
{
    public class StudentAttendanceVM
    {
        public StudentsAttendance StudentsAttendance { get; set; }
        public List<StudentsAttendance> StudentsAttendances { get; set; }
    }
}
