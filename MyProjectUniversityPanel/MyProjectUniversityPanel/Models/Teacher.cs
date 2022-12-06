using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectUniversityPanel.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        public Gender Gender { get; set; }
        public int GenderId { get; set; }

        public string Degree { get; set; }
       
       
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }
        public DateTime JoiningDate { get; set; }




    }
}
