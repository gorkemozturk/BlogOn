using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.User).ToListAsync();

            return View(posts);
        }
    }
}