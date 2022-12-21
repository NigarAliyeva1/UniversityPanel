namespace MyProjectUniversityPanel.Models
{
    public class TeacherGroups
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public bool IsDeactive { get; set; }
    }
}
