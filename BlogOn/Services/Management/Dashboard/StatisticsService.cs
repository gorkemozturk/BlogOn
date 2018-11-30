using BlogOn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Services.Management.Dashboard
{
    public class StatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DailyComments()
        {
            return _context.Comments.Where(c => c.CreatedAt.Day == DateTime.Now.Day).ToList().Count();
        }

        public int DailyUsers()
        {
            return _context.ApplicationUsers.Where(u => u.CreatedAt.Day == DateTime.Now.Day).ToList().Count();
        }

        public int TotalCategories()
        {
            return _context.Categories.ToList().Count();
        }

        public int TotalTags()
        {
            return _context.Tags.ToList().Count();
        }
    }
}
