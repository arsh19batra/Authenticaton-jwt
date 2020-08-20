using System.ComponentModel.DataAnnotations;

namespace proj.Dtos
{
    public class DetailAuthenticateDto
    {
        [Required]
        public  string uname{get; set;}
        [Required]
        public string password{get; set;}
    }
}