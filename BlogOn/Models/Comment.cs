using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Models
{
    public class Comment
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "User")]
        public string UserID { get; set; }

        [Display(Name = "Post")]
        public int PostID { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Body { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }
    }
}
