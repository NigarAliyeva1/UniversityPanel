using MyProjectUniversityPanel.Models;
using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public  bool RememberMe { get; set; }



    }
}
