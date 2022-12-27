using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectUniversityPanel.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public string HomeFile { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        [NotMapped]
        public IFormFile UploadFile { get; set; }
        [NotMapped]
        public IFormFile UploadHomeFile { get; set; }

        public bool IsDeactive { get; set; }
        public bool IsHomework { get; set; }
        public DateTime Deadline { get; set; }
    }
}
