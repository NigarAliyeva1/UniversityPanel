using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace MyProjectUniversityPanel.Models
{
    public class Student
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
        public DateTime AdmissionDate { get; set; }
    }
}
