using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MicroVerse.ViewModels;
using System.Security.Claims;

namespace MicroVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PostHelper _postHelper;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
            _postHelper = new PostHelper(_context);
        }

        // GET: api/Post
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPost()
        {
            return Json(_postHelper.GetPosts());
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost(Guid id)
        {
            var post = _postHelper.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return Json(post);
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
            var userId = HttpContext.User.Identity.Name;
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
            var userId = HttpContext.User.Identity.Name;
            Post reactsTo = await _context.Post.FindAsync(postId);
            Post post = new Post(text, userId, reactsTo);
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return new LocalRedirectResult("/Home/Index");
        }

        // POST: api/Post/DeletePost/5
        [HttpPost("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return new LocalRedirectResult("/Home/Index");
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

        // PATCH: api/Post/Flag/5
        [HttpPatch("Flag/{id}")]
        public async Task<IActionResult> FlagPost(Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Activation = Activation.flagged;

            return await PutPost(id, post);
        }

        // PATCH: api/Post/Flag/5
        [HttpPatch("Unflag/{id}")]
        public async Task<IActionResult> UnflagPost(Guid id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Activation = Activation.active;

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
        public IActionResult SearchPosts(String phrase)
        {
            var posts = (new SearchHelper(_context)).Posts(phrase);

            return Json(posts);
        }
        

        private bool PostExists(Guid id)
        {
            return _context.Post.Any(e => e.Id == id);
        }


        // Create a post using token for user authentication
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AuthPost")]
        //api/Post/AuthPost
        public async Task<IActionResult> AuthPost([FromBody] AuthPostViewModel model)
        {
            User user = new();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            Post post = new Post(model.Body, user.UserName);
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AuthPost", new { id = post.Id }, post);

        }
    }
}
