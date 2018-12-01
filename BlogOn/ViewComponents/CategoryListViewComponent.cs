using BlogOn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogOn.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync();

            return View(categories);
        }
    }
}
