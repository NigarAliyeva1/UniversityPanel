using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MyProjectUniversityPanel.ViewModels
{
    public class ChangeRoleVM
    {
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }
     
        public string Username { get; set; }
      
    }
}
    