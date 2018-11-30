using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogOn.Data;
using BlogOn.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogOn.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public IActionResult Get()
        {
            var comments = _context.Comments.ToList();

            return Ok(comments);
        }

        // GET: api/Comments/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var comments = _context.Comments.Where(c => c.PostID == id).ToList();

            return Ok(comments);
        }

        // POST: api/Comments
        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            comment.UserID = claim.Value;
            comment.CreatedAt = DateTime.Now;

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
