using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = StaticEnvironments.Administrator)]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }
    }
}