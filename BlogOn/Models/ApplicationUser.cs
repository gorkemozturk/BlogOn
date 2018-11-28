using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Authorized")]
        public bool IsAuthorized { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        public string FullName() {
            return FirstName + " " + LastName;
        }
    }
}
