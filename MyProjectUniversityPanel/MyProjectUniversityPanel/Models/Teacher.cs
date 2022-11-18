using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectUniversityPanel.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public Gender Gender { get; set; }
        public int GenderId { get; set; }
        public string Degree { get; set; }
        public DateTime CreateTime { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }

    }
}
