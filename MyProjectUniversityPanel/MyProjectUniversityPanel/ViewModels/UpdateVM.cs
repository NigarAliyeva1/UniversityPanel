using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.ViewModels
{
    public class UpdateVM
    {
        [Required]
        public string FullName { get; set; }


        [Required]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IFormFile Photo { get; set; }

       
    }
}
