using System.ComponentModel.DataAnnotations;

namespace MyProjectUniversityPanel.Models
{
    public class DepartmentDetail
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Capacity { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public bool IsDeactive { get; set; }
    }
}
