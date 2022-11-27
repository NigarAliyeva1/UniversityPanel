﻿using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.ViewModels
{
    public class ResetPasswordVM
    {
       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
    }
}
