using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace MyProjectUniversityPanel.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string File { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }
        public int GenderId { get; set; }

        public Designation Designation { get; set; }
        public int DesignationId { get; set; }

        [Required]
        public string Education { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Salary { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        [NotMapped]
        public IFormFile UploadFile { get; set; }
        public bool IsDeactive { get; set; }
        public DateTime JoiningDate { get; set; }

        public List<Salary> Salaries { get; set; }



    }
}
