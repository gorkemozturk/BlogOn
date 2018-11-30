using System;
using System.ComponentModel.DataAnnotations;

namespace BlogOn.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}
