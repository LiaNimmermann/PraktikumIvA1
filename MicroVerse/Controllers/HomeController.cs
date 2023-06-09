using Microsoft.AspNetCore.Mvc;
using MicroVerse.Models;
using MicroVerse.ViewModels;
using System.Diagnostics;
using MicroVerse.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private IEnumerable<User> _users = new List<User>();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var postsList = _context.Post
                .OrderByDescending(post => post.CreatedAt)
                .Join(
                    _context.Users,
                    post => post.AuthorId,
                    user => user.UserName,
                    (post, user) => new { DisplayName = user.DisplayedName, Post = post }
                ) 
                .Select(post => new PostViewModel
                    (
                        post.Post.Body,
                        null,
                        post.Post.CreatedAt,
                        post.DisplayName,
                        post.Post.AuthorId,
                        post.Post.Votes.Where(x => x.Upvote > 0).Count(),
                        post.Post.Votes.Where(x => x.Upvote < 0).Count()
                       
                    )).ToList();
            Random r = new Random();
            var followsList = Enumerable.Range(0,5)
                .Select(i =>
                    new User("User" + i, "User " + i, "I'm just a normal User with a normal Bio", Role.user)
                    {
                        DisplayedName = "Followed User " + i
                    }).ToList();

            var mockModel = new HomeViewModel(followsList, postsList);
            return View(mockModel);
        }

        public async Task<IActionResult> Profile(string id)
        {
            User user = new User("User1234", "User 12 34", "I'm just a normal User with a normal Bio", Role.user);
            if (id != null) {
                user = await _context.Users.FirstOrDefaultAsync(user => id == user.UserName);
            }
            //TODO: Create a ProfileViewModel with real Data
            //Creating Mock Data

            var postsList = InitializePostList(user);

            var followsList = _context.Follows.Where(f => f.FollowingUserId == id);
            var followerList = _context.Follows.Where(f => f.FollowedUserId == id);
            Random r = new Random();
            var model = new ProfileViewModel
            (
                user.UserName.ToString(),
                user.DisplayedName,
                user.Bio,
                followerList.Count(),
                followsList.Count(),
                postsList
            );

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<PostViewModel> InitializePostList(User user) {
            if (!(user.UserName == "User1234")) {
                return _context.Post
                    .Where(post => post.AuthorId == user.UserName)
                    .OrderByDescending(post => post.CreatedAt)
                    .Select(post => new PostViewModel
                            (
                                post.Body,
                                null,
                                post.CreatedAt,
                                user.DisplayedName,
                                user.UserName.ToString(),
                                post.Votes.Where(x => x.Upvote > 0).Count(),
                                post.Votes.Where(x => x.Upvote < 0).Count()
                            )).ToList();
            }

            var r = new Random();
            return Enumerable.Range(0, 5)
                .Select(i => new PostViewModel
                        (
                            "This is a Post " + i + " by " + user.DisplayedName,
                            null,
                            DateTime.Now,
                            user.DisplayedName,
                            user.UserName.ToString(),
                            (int)r.Next(0, 100),
                            (int)r.Next(0, 50)
                        )).ToList();
        }
    }
}
