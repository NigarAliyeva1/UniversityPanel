using System;

namespace MyProjectUniversityPanel.Models
{
    public class StudentsAttendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime Date { get; set; }
        public bool IsChecked { get; set; }

    }
}
