using System.ComponentModel.DataAnnotations;

namespace proj.Models
{
    public class Detail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string uname { get; set; }
        [Required]
        [MinLength(20)]
        public string password { get; set; }
    }
}