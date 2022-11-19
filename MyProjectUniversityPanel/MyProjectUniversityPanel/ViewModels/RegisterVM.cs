using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static MyProjectUniversityPanel.Helpers.Helper;

namespace MyProjectUniversityPanel.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }


        [Required]
        public string UserName { get; set; }

        public string Image { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        
        public string ReturnUrl { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }
        public IFormFile Photo { get; set; }

    }
}
