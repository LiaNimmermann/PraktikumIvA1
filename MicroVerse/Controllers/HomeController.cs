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
        private readonly PostHelper _postHelper;
        private readonly UserManager<User> _userManager;
        private UserController _userController;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _postHelper = new PostHelper(_context);
        }

        public IActionResult Index()
        {
        	var users = _context.Users
        		.ToList();
            var postsList = _postHelper.GetPosts()
            	.Select(post 
            		=> new PostViewModel(post, users))
            	.ToList();
                
            var currentUser = User.Identity.Name;
            var followsList = _context.Follows
                .Where(f => f.FollowingUserId == currentUser)
                .Join(
                    _context.Users,
                    follow => follow.FollowedUserId,
                    user => user.UserName,
                    (follow, user) => user
                ).ToList();

            var model = new HomeViewModel(followsList, postsList);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            User user = new User("User1234", "User 12 34", "I'm just a normal User with a normal Bio", Role.user);
            if (id != null) {
                user = await _context.Users.FirstOrDefaultAsync(user => id == user.UserName) ?? user;
            }
            var postsList = InitializePostList(user);

            var followsList = _context.Follows.Where(f => f.FollowingUserId == id);
            var followerList = _context.Follows.Where(f => f.FollowedUserId == id);
            var follows = _context.Follows.Where(f => f.FollowedUserId == id && f.FollowingUserId == User.Identity.Name);
            var model = new ProfileViewModel
            (
                user.UserName.ToString(),
                user.DisplayedName,
                user.Bio,
                followerList.Count(),
                followsList.Count(),
                postsList,
                (follows.Count() == 1),
                _userController.GetUserRole(user.UserName).Result                
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

        private List<PostViewModel> InitializePostList(User user) 
        {
        	var users = _context.Users.ToList();
        	
            return _postHelper.GetPostsByUserAndFollows(user.UserName)
                .Select(post => new PostViewModel(post, users))
                .ToList();
        }

        [Authorize]
        [HttpPost] //Search functionality (using Displayed Name)
        public IActionResult Search(string searchTerm)
        {
            var currentUser = User.Identity.Name;
            var followsList = _context.Follows
            	.Where(f => f.FollowingUserId == currentUser)
            	.ToList();
        
            var searchResults = (new SearchHelper(_context))
            	.Users(searchTerm);
            var asViews = searchResults
            	.Select(u => new UserBadgeViewModel
            	(
            		u.UserName,
            		u.DisplayedName,
            		followsList.Any(f =>f.FollowedUserId == u.UserName)
            	))
            	.ToList();
            return View("SearchResult", asViews);
        }

        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult Moderation()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUserOverview()
        {
            List<UserWithRole> users = new List<UserWithRole>();

            return View(users);
        }
    }
}
