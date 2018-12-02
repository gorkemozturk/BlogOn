using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string keyword)
        {
            var posts = from p in _context.Posts
                         select p;

            if (keyword == null)
                return NotFound();

            if (!String.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(p => p.Title.Contains(keyword)).Include(p => p.User).Include(p => p.Category);
            }

            return View(await posts.ToListAsync());
        }

        [Route("Search/Category/{id}/{slug}")]
        public async Task<IActionResult> Category(int? id, string slug)
        {
            if (id == null || slug == null)
                return NotFound();

            var posts = await _context.Posts.Include(p => p.User).Include(p => p.Category).Where(p => p.CategoryID == id).ToListAsync();

            return View(posts);
        }
    }
}