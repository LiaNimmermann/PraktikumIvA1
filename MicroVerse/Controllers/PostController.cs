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
        public async Task<IActionResult> PutPost(Guid id, Post post)
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
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] String text)
        {
            string userId = HttpContext.User.Identity.Name;
            Post post = new Post(text, userId);
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return new LocalRedirectResult("/Home/Index");
        }

        [HttpPost("{reactsTo}/{text}")]
        public async Task<IActionResult> ReactToPost(Guid reactsTo, String text)
        {
            var userId = User.Identity.Name;
            var post = new Post(text, userId)
            {
                ReactsTo = await _context.Post.FindAsync(reactsTo)
            };
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/React
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("React")]
        public async Task<IActionResult> React([FromForm] String text, [FromForm] Guid postId)
        {
            string userId = HttpContext.User.Identity.Name;
            Post reactsTo = await _context.Post.FindAsync(postId);
            Post post = new Post(text, userId, reactsTo);
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return new LocalRedirectResult("/Home/Index");
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
        public async Task<IActionResult> BlockPost(Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Activation = Activation.blocked;

            return await PutPost(id, post);
        }

        // PATCH: api/Post/Up/user@id.com/5
        [HttpPatch("Up/{user}/{id}")]
        public async Task<IActionResult> UpvotePost(String user, Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Votes.Add(new Vote(post.Id, user, 1));

            return await PutPost(id, post);
        }

        // PATCH: api/Post/Down/user@id.com/5
        [HttpPatch("Down/{user}/{id}")]
        public async Task<IActionResult> DownvotePost(String user, Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Votes.Add(new Vote(post.Id,user,-1));

            return await PutPost(id, post);
        }

        [HttpGet("Search/{phrase}")]
        public async Task<IActionResult> SearchPosts(String phrase)
        {
            var posts = _context.Post
                .AsEnumerable()
                .Where(p => p.FuzzyMatches(phrase));

            return Json(posts);
        }

        private bool PostExists(Guid id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
