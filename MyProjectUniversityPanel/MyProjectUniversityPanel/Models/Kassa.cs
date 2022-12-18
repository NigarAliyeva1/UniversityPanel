using System;

namespace MyProjectUniversityPanel.Models
{
    public class Kassa
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public string LastModifiedBy { get; set; }
        public int LastModifiedMoney { get; set; }
        public string LastModifiedFor { get; set; }
        public bool IsDeactive { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
