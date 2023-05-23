using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            return await _context.Post.ToListAsync();
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Post.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Post/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockPost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Activation = Activation.blocked;

            return await PutPost(id, post);
        }

        [HttpPatch("Up/{user}/{id}")]
        public async Task<IActionResult> UpvotePost(String user, int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Votes.Add(new Vote { Upvote = 1, UserId = user, PostId = post.Id });

            return await PutPost(id, post);
        }

        [HttpPatch("Down/{user}/{id}")]
        public async Task<IActionResult> DownvotePost(String user, int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Votes.Add(new Vote { Upvote = -1, UserId = user, PostId = post.Id });

            return await PutPost(id, post);
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
