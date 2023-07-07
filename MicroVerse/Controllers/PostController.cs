using Microsoft.AspNetCore.Mvc;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MicroVerse.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MicroVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly PostHelper _postHelper;
        private readonly SearchHelper _searchHelper;
        private readonly UserHelper _userHelper;

        public PostController(ApplicationDbContext context)
        {
            _searchHelper = new SearchHelper(context);
            _postHelper = new PostHelper(context);
            _userHelper = new UserHelper(context);
        }

        // GET: api/Post
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPost()
            => Json(_postHelper.GetPosts());

        // GET: api/Post
        [HttpGet("by/{authorId}")]
        public ActionResult<IEnumerable<Post>> GetPost(String authorId)
            => Json(_postHelper.GetPostsByUser(authorId));

        // GET: api/Post
        [HttpGet("by/follows/{authorId}")]
        public ActionResult<IEnumerable<Post>> GetPosts(String authorId)
            => Json(_postHelper.GetPostsByUserAndFollows(authorId));

        // GET: api/Post/5
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost(Guid id)
        {
            var post = _postHelper.GetPost(id);

            if (post is null)
            {
                return NotFound();
            }

            return Json(post);
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(Guid id, Post post)
            => StatusToActionResult(await _postHelper.PutPost(id, post));

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] String text)
            => await React(text, null);

        // POST: api/React
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("React")]
        public async Task<IActionResult> React([FromForm] String text, [FromForm] Guid? postId)
        {
            _ = await _postHelper.Post(text, User?.Identity?.Name ?? "", postId);

            return new LocalRedirectResult("/Home/Index");
        }

        // POST: api/Post/DeletePost/5
        [HttpPost("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
            => await _postHelper.DeletePost(id) switch
            {
                Status.NotFound => NotFound(),
                Status.Redirect => new LocalRedirectResult("/Home/Index"),
                _ => BadRequest()
            };

        // PATCH: api/Post/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockPost(Guid id)
            => StatusToActionResult(await _postHelper.BlockPost(id));

        // PATCH: api/Post/Flag/5
        [HttpPatch("Flag/{id}")]
        public async Task<IActionResult> FlagPost(Guid id)
            => StatusToActionResult(await _postHelper.FlagPost(id));

        // PATCH: api/Post/Flag/5
        [HttpPatch("Unflag/{id}")]
        public async Task<IActionResult> UnflagPost(Guid id)
            => StatusToActionResult(await _postHelper.ActivatePost(id));

        // PATCH: api/Post/Up/user@id.com/5
        [HttpPatch("Up/{user}/{id}")]
        public async Task<IActionResult> UpvotePost(String user, Guid id)
            => StatusToActionResult(await _postHelper.VotePost(id, user, Vote.Votes.Up));

        // PATCH: api/Post/Down/user@id.com/5
        [HttpPatch("Down/{user}/{id}")]
        public async Task<IActionResult> DownvotePost(String user, Guid id)
            => StatusToActionResult(await _postHelper.VotePost(id, user, Vote.Votes.Down));

        [HttpGet("Search/{phrase}")]
        public IActionResult SearchPosts(String phrase)
            => Json(_searchHelper.Posts(phrase));

        // Create a post using token for user authentication
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AuthPost")]
        //api/Post/AuthPost
        public async Task<IActionResult> AuthPost([FromBody] AuthPostViewModel model)
        {
            User user = new();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            user = await _userHelper.GetUserId(userId);
            if (user is null)
            {
                return NotFound();
            }

            var post = await _postHelper.Post(model.Body, user.UserName, null);

            return CreatedAtAction("AuthPost", new { id = post.Id }, post);
        }

        private IActionResult StatusToActionResult(Status status)
            => status switch
            {
                Status.NoContent => NoContent(),
                Status.NotFound => NotFound(),
                _ => BadRequest()
            };
    }
}
