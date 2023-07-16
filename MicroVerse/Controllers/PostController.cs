using Microsoft.AspNetCore.Mvc;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MicroVerse.ViewModels;
using System.Security.Claims;

namespace MicroVerse.Controllers
{
    // The PostController provides an API for posts
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

        // get a list with all posts
        // GET: api/Post
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPost()
            => Json(_postHelper.GetPosts());

        // get a list with all posts by a certain user
        // GET: api/Post
        [HttpGet("by/{authorId}")]
        public ActionResult<IEnumerable<Post>> GetPost(String userName)
            => Json(_postHelper.GetPostsByUser(userName));

        // get a list with all posts by a certain user and the users that he follows
        // GET: api/Post
        [HttpGet("by/follows/{authorId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(String userName)
            => Json(await _postHelper.GetPostsByUserAndFollows(userName));

        // get a single post
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

        // modify an existing post
        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(Guid id, Post post)
            => StatusToActionResult(await _postHelper.PutPost(id, post));

        // create a new post
        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] String text)
            => await React(text, null);

        // create a new post that reacts to an existing post
        // POST: api/React
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("React")]
        public async Task<IActionResult> React([FromForm] String text, [FromForm] Guid? postId)
        {
            _ = await _postHelper.Post(text, User?.Identity?.Name ?? "", postId);

            return new LocalRedirectResult("/Home/Index");
        }

        // delete an existing post
        // POST: api/Post/DeletePost/5
        [Authorize]
        [HttpPost("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
            => await _postHelper.DeletePost(id) switch
            {
                Status.NotFound => NotFound(),
                Status.Redirect => new LocalRedirectResult("/Home/Index"),
                _ => BadRequest()
            };

        // block a post
        // PATCH: api/Post/5
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockPost(Guid id)
            => StatusToActionResult(await _postHelper.BlockPost(id));

        // flag a post
        // PATCH: api/Post/Flag/5
        [Authorize]
        [HttpPost("Flag/{id}")]
        public async Task<IActionResult> FlagPost(Guid id)
            => StatusToActionResult(await _postHelper.FlagPost(id));

        // unflag a post
        // PATCH: api/Post/Unflag/5
        [Authorize]
        [HttpPost("Unflag/{id}")]
        public async Task<IActionResult> UnflagPost(Guid id)
            => StatusToActionResult(await _postHelper.ActivatePost(id));

        // get all flagged posts
        // GET: api/Post/Flagged
        [Authorize]
        [HttpGet("Flagged")]
        public IActionResult GetFlaggedPosts()
            => Json(_postHelper.GetFlaggedPosts());

        // upvote a post
        // POST: api/Post/Up/user@id.com/5
        [Authorize]
        [HttpPost("Up/{user}/{id}")]
        public async Task<IActionResult> UpvotePost(String user, Guid id)
            => StatusToActionResult(await _postHelper.VotePost(id, user, Vote.Votes.Up));

        // downvote a post
        // POST: api/Post/Down/user@id.com/5
        [Authorize]
        [HttpPost("Down/{user}/{id}")]
        public async Task<IActionResult> DownvotePost(String user, Guid id)
            => StatusToActionResult(await _postHelper.VotePost(id, user, Vote.Votes.Down));

        // Create a post using token for user authentication
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AuthPost")]
        //api/Post/AuthPost
        public async Task<IActionResult> AuthPost([FromBody] AuthPostViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userHelper.GetUserId(userId);
            if (user is null)
            {
                return NotFound();
            }

            var post = await _postHelper.Post(model.Body, user.UserName, null);

            return CreatedAtAction("AuthPost", new { id = post.Id }, post);
        }

        // get all statistics about posts
        [HttpGet("UserPostingStats")]
        public async Task<IActionResult> UserPostingStatistics()
        {
            var groups = _postHelper.GetPostsGroupedByUser();
            var name = new List<String>();
            var allTime = new List<Int32>();
            var last7 = new List<Int32>();
            var today = new List<Int32>();
            foreach (var g in groups) {
                name.Add(g.First().AuthorId);
                allTime.Add(g.Count());
                last7.Add(g.Where(p => DateTime.Today - p.CreatedAt < new TimeSpan(7, 0, 0, 0))
                          .Count());
                today.Add(g.Where(p => DateTime.Today - p.CreatedAt < new TimeSpan(1, 0, 0, 0))
                          .Count());
            }

            var mostFollowed = (await _userHelper.GetUsersWithFollows())
                .OrderByDescending(u => u.Followers.Count())
                .Take(5);

            var mostUpvoted = groups
                .OrderByDescending
                (
                    g => g.Select(p => p.Votes)
                    .Select(v => v
                            .Where(v => v.Upvote == Vote.Votes.Up)
                            .Count())
                    .Sum()
                )
                .Take(5);

            var mostDownvoted = groups
                .OrderByDescending
                (
                    g => g.Select(p => p.Votes)
                    .Select(v => v
                            .Where(v => v.Upvote == Vote.Votes.Down)
                            .Count())
                    .Sum()
                )
                .Take(5);

            var stats = new {
                UserPosting = new {
                    Labels = name,
                    PostsAllTime = allTime,
                    PostsLastWeek = last7,
                    PostsToday = today
                },
                MostFollowedUsers = new {
                    Labels = mostFollowed.Select(u => u.UserName).ToList(),
                    Followers = mostFollowed.Select(u => u.Followers.Count()).ToList(),
                    Following = mostFollowed.Select(u => u.Following.Count()).ToList()
                },
                MostUpvotedPost = new {
                    Labels = mostUpvoted.Select(g => g.First().AuthorId),
                    Upvotes = mostUpvoted.Select
                    (
                        g => g.Select
                        (
                            p => p.Votes.Select(v => v.Upvote)
                            .Where(v => v == Vote.Votes.Up)
                            .Count()
                        ).Sum()
                    ),
                    Downvotes = mostUpvoted.Select
                    (
                        g => g.Select
                        (
                            p => p.Votes.Select(v => v.Upvote)
                            .Where(v => v == Vote.Votes.Down)
                            .Count()
                        ).Sum()
                    )
                },
                MostDownvotedPost = new {
                    Labels = mostDownvoted.Select(g => g.First().AuthorId),
                    Upvotes = mostDownvoted.Select
                    (
                        g => g.Select
                        (
                            p => p.Votes.Select(v => v.Upvote)
                            .Where(v => v == Vote.Votes.Up)
                            .Count()
                        ).Sum()
                    ),
                    Downvotes = mostDownvoted.Select
                    (
                        g => g.Select
                        (
                            p => p.Votes.Select(v => v.Upvote)
                            .Where(v => v == Vote.Votes.Down)
                            .Count()
                        ).Sum()
                    )
                }
            };

            return Json(stats);
        }

        // helper method to convert a Status (result of PostHelper methods) into
        // an ActionResult
        private IActionResult StatusToActionResult(Status status)
            => status switch
            {
                Status.NoContent => NoContent(),
                Status.NotFound => NotFound(),
                _ => BadRequest()
            };

        // Get user posts
        // GET: api/Post/fromuser?userName=xxxxx
        [HttpGet]
        [Route("fromuser")]
        public ActionResult<IEnumerable<Post>> GetUserPost(String userName)
        => Json(_postHelper.GetPostsByUser(userName));
    }
}
