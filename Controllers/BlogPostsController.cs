using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortZer0.Data;
using PortZer0.Models;

namespace PortZer0.Controllers
{
    [Route("api/[controller]")]
    public class BlogPostsController : Controller
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

        [HttpPost("/BlogPosts/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
                return NotFound();

            _context.BlogPost.Remove(blogPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Manage");
        }

        [HttpGet("/BlogPosts/Manage")]
        public async Task<IActionResult> Manage()
        {
            var posts = await _context.BlogPost.ToListAsync();
            return View(posts);
        }

        [HttpPost("/BlogPosts/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] BlogPost blogPost)
        {
            if (!ModelState.IsValid) return View(blogPost); // Add this for debugging/feedback

            _context.BlogPost.Add(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        [HttpGet("/BlogPosts/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
                return NotFound();

            return View(blogPost);
        }

        [HttpPost("/BlogPosts/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(blogPost);

            try
            {
                _context.Update(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BlogPost.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction("Manage");
        }


    }
}
