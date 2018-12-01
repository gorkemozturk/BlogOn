using BlogOn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.Services.Client.Comment
{
    public class CommentsService
    {
        private readonly ApplicationDbContext _context;

        public CommentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CommentCounts(int id)
        {
            return _context.Comments.Where(c => c.PostID == id).ToList().Count();
        }
    }
}
