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

        // The Route for the Home Page
        public async Task<IActionResult> Index()
        {
            var currentUser = GetCurrentUser();
            var users = _userHelper.GetUsers();
            var postsList = _postHelper.GetPosts()
                .PostsToViewModel(users, currentUser);

            var followsList = (await _followsHelper.GetFollowings(currentUser))
                .ToList();

            var model = new HomeViewModel(followsList, postsList);
            return View(model);
        }

        // The Followed Feed shows a logged-in user his own posts and all posts
        // by other users that he followed.
        [Authorize]
        public async Task<IActionResult> FollowedFeed()
        {
            var currentUser = GetCurrentUser();
            var followsList = (await _followsHelper.GetFollowings(currentUser)).ToList();
            var postList = _postHelper.GetPostsByUserAndFollows
                (
                    currentUser,
                    followsList.Select(u => u.UserName).Append(currentUser)
                ).PostsToViewModel(_userHelper.GetUsers(), currentUser);

            var model = new HomeViewModel(followsList, postList);
            return View(model);
        }

        // Shows a user's Profile: his username, displayed name, number of
        // followers and followings, bio and posts by the user and all users he
        // follows. 
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var users = _userHelper.GetUsers();
            var user = await _userHelper.GetUserWithFollows(id);
            var currentUser = GetCurrentUser();

            if (user is null)
            {
                return NotFound();
            }

            var follows = await _followsHelper.Follows(currentUser, id);

            var postsList = _postHelper.GetPostsByUserAndFollows
                (
                    user.UserName,
                    user.Following
                ).PostsToViewModel(users, currentUser);

            var model = new ProfileViewModel
                (
                    user,
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

        // Searches for a user
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
                            u,
                            follows.Any(f => f == u.UserName)
                        ))
                .ToList();

            return View("SearchResult", searchResults);
        }

        // The moderation page
        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult Moderation()
        {
            var currentUser = GetCurrentUser();
            var users = _userHelper.GetUsers();
            var postsList = _postHelper.GetFlaggedPosts()
                .PostsToViewModel(users, currentUser);
            return View(postsList);
        }

        // The statistics page
        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult Statistics() => View();

        // An overview of users for admins
        [Authorize(Roles = "Admin")]
        public IActionResult AdminUserOverview()
            => View(_userHelper.GetUserWithRole().ToList());

        private string GetCurrentUser() => User?.Identity?.Name ?? "";
    }
}
