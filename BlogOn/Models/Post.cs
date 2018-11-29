using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Models
{
    public class Post
    {
        public int ID { get; set; }

        [Display(Name = "User")]
        public string UserID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        public string Body { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
    }
}
