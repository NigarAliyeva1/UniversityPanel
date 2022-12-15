using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.ViewModels
{
    public class ChangeRoleVM
    {
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }
        public string FullName { get; set; }

        public string Username { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }
    }
}
    