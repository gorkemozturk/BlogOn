using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Models;
using BlogOn.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogOn.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = StaticEnvironments.Administrator)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || id.Trim().Length == 0)
                return NotFound();

            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(user);

            ApplicationUser existingUser = await _context.ApplicationUsers.Where(u => u.Id == id).FirstOrDefaultAsync();

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.IsAuthorized = user.IsAuthorized;

            if (user.IsAuthorized == true)
                await _userManager.AddToRoleAsync(existingUser, StaticEnvironments.Administrator);
            else
                await _userManager.RemoveFromRoleAsync(existingUser, StaticEnvironments.Administrator);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Lockout(string id)
        {
            if (id == null || id.Trim().Length == 0)
                return NotFound();

            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lockout(string id, ApplicationUser user)
        {
            if (id != user.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(user);

            ApplicationUser existingUser = await _context.ApplicationUsers.Where(u => u.Id == id).FirstOrDefaultAsync();

            existingUser.LockoutEnd = DateTime.Now.AddYears(100);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Unlock(string id)
        {
            if (id == null || id.Trim().Length == 0)
                return NotFound();

            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id, ApplicationUser user)
        {
            if (id != user.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(user);

            ApplicationUser existingUser = await _context.ApplicationUsers.Where(u => u.Id == id).FirstOrDefaultAsync();

            existingUser.LockoutEnd = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}