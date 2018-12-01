using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Category/Posts/{id}/{slug}")]
        public async Task<IActionResult> Posts(int? id, string slug)
        {
            if (id == null || slug ==null)
                return NotFound();

            var posts = await _context.Posts.Include(p => p.User).Include(p => p.Category).Where(p => p.CategoryID == id).ToListAsync();

            return View(posts);
        }
    }
}