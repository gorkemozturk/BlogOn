using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Models;
using BlogOn.Models.ViewModels.Account;
using BlogOn.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogOn.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registration)
        {
            if (!ModelState.IsValid)
                return View(registration);

            var user = new ApplicationUser()
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.Email,
                Email = registration.Email,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registration.Password);

            if (!await _roleManager.RoleExistsAsync(StaticEnvironments.Administrator))
                await _roleManager.CreateAsync(new IdentityRole(StaticEnvironments.Administrator));

            if (!await _roleManager.RoleExistsAsync(StaticEnvironments.User))
                await _roleManager.CreateAsync(new IdentityRole(StaticEnvironments.User));

            if (_context.Users.ToList().Count() == 1)
            {
                await _userManager.AddToRoleAsync(user, StaticEnvironments.Administrator);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, StaticEnvironments.User);
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors.Select(e => e.Description))
                {
                    ModelState.AddModelError("", error);
                }

                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                login.Email, login.Password,
                login.RememberMe, false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login error!");
                return View();
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }
    }
}