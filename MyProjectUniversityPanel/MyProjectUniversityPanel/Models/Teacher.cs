using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectUniversityPanel.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public Gender Gender { get; set; }
        public int GenderId { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }
        public DateTime JoiningDate { get; set; }
        public virtual ICollection<DepartmentDetail> DepartmentDetails { get; set; }
        public List<TeacherGroups> teacherGroups { get; set; }



    }
}
