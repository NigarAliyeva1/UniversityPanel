using System;
using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.Models
{
    public class Income
    {
        public int Id { get; set; }
        public int Money { get; set; }
        [Required]
        public string For { get; set; }
        public DateTime Date { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public bool IsDeactive { get; set; }
    }
}
