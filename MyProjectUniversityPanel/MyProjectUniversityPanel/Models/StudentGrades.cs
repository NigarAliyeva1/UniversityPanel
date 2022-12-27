namespace MyProjectUniversityPanel.Models
{
    public class StudentGrades
    {
        public int Id { get; set; }
        public int Midterm { get; set; }
        public int Quiz { get; set; }
        public int Presentation { get; set; }
        public int Exam { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public bool IsMidterm{ get; set; }
        public bool IsQuiz{ get; set; }
        public bool IsPresentation { get; set; }
        public bool IsExam{ get; set; }


    }
}
