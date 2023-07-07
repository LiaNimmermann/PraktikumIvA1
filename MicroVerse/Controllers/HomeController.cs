using Microsoft.AspNetCore.Mvc;
using MicroVerse.Models;
using MicroVerse.ViewModels;
using System.Diagnostics;
using MicroVerse.Data;
using MicroVerse.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MicroVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly PostHelper _postHelper;
        private readonly UserHelper _userHelper;
        private readonly SearchHelper _searchHelper;
        private readonly FollowsHelper _followsHelper;

        public HomeController
        (
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<User> userManager,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _userHelper = new UserHelper(_context, _userManager);
            _postHelper = new PostHelper(_context);
            _searchHelper = new SearchHelper(_context);
            _followsHelper = new FollowsHelper(_context);
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = User?.Identity?.Name ?? "";
            var users = await _userHelper.GetUsers();
            var postsList = _postHelper.GetPosts()
                .Select(post => new PostViewModel(post, users)
                {
                    VoteByUser = post.VotingByUser(currentUser)
                })
                .ToList();

            var followsList = (await _followsHelper.GetFollowings(currentUser))
                .ToList();

            var model = new HomeViewModel(followsList, postsList);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userHelper.GetUser(id);

            if (user is null)
            {
                return NotFound();
            }

            var postsList = await InitializePostList(user);

            var currentUser = User?.Identity?.Name ?? "";

            var followsList = await _followsHelper.GetFollowings(id);
            var followerList = await _followsHelper.GetFollowers(id);
            var follows = await _followsHelper.Follows(currentUser, id);

            var model = new ProfileViewModel
                (
                    user.UserName,
                    user.DisplayedName,
                    user.Bio,
                    followerList,
                    followsList,
                    postsList,
                    follows,
                    await _userHelper.GetUserRole(user.UserName)
                );

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private async Task<List<PostViewModel>> InitializePostList(User user)
        {
            var users = await _userHelper.GetUsers();
            var currentUser = User?.Identity?.Name ?? "";

            return _postHelper.GetPostsByUserAndFollows(user.UserName)
                .Select(post => new PostViewModel(post, users)
                {
                    VoteByUser = post.VotingByUser(currentUser)
                })
                .ToList();
        }

        [Authorize]
        [HttpPost] //Search functionality (using Displayed Name)
        public async Task<IActionResult> Search(string searchTerm)
        {
            var currentUser = User?.Identity?.Name;
            var follows = await _context.Follows
                .Where(f => f.FollowingUserId == currentUser)
                .Select(f => f.FollowedUserId)
                .ToListAsync();

            var searchResults = _searchHelper
                .Users(searchTerm)
                .Select(u => new UserBadgeViewModel
                        (
                            u.UserName,
                            u.DisplayedName,
                            follows.Any(f => f == u.UserName)
                        ))
                .ToList();

            return View("SearchResult", searchResults);
        }

        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult Moderation()
        {
            return View();
        }

        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult Statistics()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUserOverview()
        {
            var users = new List<UserWithRole>();

            return View(users);
        }
    }
}
