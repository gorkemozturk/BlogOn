using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Models;
using BlogOn.Models.ViewModels;
using BlogOn.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            return View(await _context.Posts.Include(p => p.Category).Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToListAsync());
        }

        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            model = new PostCreateViewModel()
            {
                Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync(),
                Post = new Post()
            };

            return View(model);
        }

        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(PostCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var slug = model.Post.Title.ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            slug = Regex.Replace(slug, @"\s", "-");

            model.Post.UserID = claim.Value;
            model.Post.CreatedAt = DateTime.Now;
            model.Post.Slug = slug;

            try
            {
                _context.Posts.Add(model.Post);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Post post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Body,IsActive")] Post post)
        {
            if (id != post.ID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(post);

            var slug = post.Title.ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            slug = Regex.Replace(slug, @"\s", "-");

            post.Slug = slug;

            try
            {
                _context.Update(post);
                _context.Entry(post).Property("CreatedAt").IsModified = false;
                _context.Entry(post).Property("UserID").IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Post post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Post post = await _context.Posts.FindAsync(id);

            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}