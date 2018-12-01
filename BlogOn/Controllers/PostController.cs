using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Models;
using BlogOn.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Post/Details/{year:min(2000)}/{month:range(1,12)}/{id}/{slug}")]
        public async Task<IActionResult> Details(int? id, int? month, int? year, string slug)
        {
            if (id == null || month == null || year == null || slug == null)
                return NotFound();

            PostViewModel model = new PostViewModel()
            {
                Post =  await _context.Posts.Include(p => p.Category).Include(p => p.User).FirstOrDefaultAsync(p => p.ID == id),
                Comments =  await _context.Comments.Include(c => c.User).Where(c => c.PostID == id).Where(c => c.IsActive == true).ToListAsync(),
                Comment = new Comment()
            };

            return View(model);
        }
    }
}