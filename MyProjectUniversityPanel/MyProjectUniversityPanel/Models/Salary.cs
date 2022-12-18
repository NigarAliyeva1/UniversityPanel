using System;

namespace MyProjectUniversityPanel.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public int Money { get; set; }

        public Staff Staff { get; set; }
        public int StaffId { get; set; }

        public DateTime Date { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public bool IsDeactive { get; set; }

    }
}
