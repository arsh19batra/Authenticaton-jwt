using System.ComponentModel.DataAnnotations;

namespace proj.Dtos
{
    public class DetailCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string uname { get; set; }
        [Required]
        public string password { get; set; }
    }
}