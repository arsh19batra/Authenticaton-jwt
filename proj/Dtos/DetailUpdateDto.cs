using System.ComponentModel.DataAnnotations;

namespace proj.Dtos
{
    public class DetailUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string uname { get; set; }
        [Required]
        public string password { get; set; }
    }
}