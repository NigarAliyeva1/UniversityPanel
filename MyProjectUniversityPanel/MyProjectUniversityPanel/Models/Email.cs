using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectUniversityPanel.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
  
    }
}
