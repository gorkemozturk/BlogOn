using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Models.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
    }
}
