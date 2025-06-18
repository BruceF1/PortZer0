using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortZer0.Data;
using PortZer0.Models;

namespace PortZer0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreateBlogPost(BlogPost blogPost)
        {
            _context.BlogPost.Add(blogPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
                return NotFound();

            return blogPost;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            return await _context.BlogPost.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, BlogPost updatedBlogPost)
        {
            var blogPostEntity = await _context.BlogPost.FindAsync(id);
            if (blogPostEntity == null)
                return NotFound();

            blogPostEntity.Title = updatedBlogPost.Title;
            blogPostEntity.Content = updatedBlogPost.Content;

            _context.Entry(blogPostEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
                return NotFound();

            _context.BlogPost.Remove(blogPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
