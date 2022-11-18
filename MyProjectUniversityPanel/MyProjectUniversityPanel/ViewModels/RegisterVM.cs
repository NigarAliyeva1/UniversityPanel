using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }


        [Required]
        public string UserName { get; set; }
        public string Role { get; set; }

        public string Image { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]


        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public IFormFile Photo { get; set; }

    }
}
