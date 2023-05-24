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
        private IEnumerable<User> _users = new List<User>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //TODO: Fill a HomeViewModel with real data
            //Creating Mock data
            var followsList = new List<User>();
            var postsList = new List<PostViewModel>();
            Random r = new Random();
            User f;
            PostViewModel p;
            for (int i = 0; i < 5; i++)
            {
                f = new User("User" + i, "User " + i, "I'm just a normal User with a normal Bio", Role.user);
                f.DisplayedName = "Followed User " + i;
                followsList.Add(f);
                p = new PostViewModel("This is a Post from User " + i, null, DateTime.Now, "Followed User " + i, "user@email.com", (int)r.Next(0, 100), (int)r.Next(0, 50));
                postsList.Add(p);
            }
            var mockModel = new HomeViewModel(followsList, postsList);
            return View(mockModel);
        }
        public IActionResult Profile(string username)
        {
            //ToDo: Create a ProfileViewModel with real Data
            //Creating Mock Data
            User user = new User("User1234", "User 12 34", "I'm just a normal User with a normal Bio", Role.user);

            List<User> followsList = new List<User>();
            var postsList = new List<PostViewModel>();
            Random r = new Random();
            PostViewModel p;
            for (int i = 0; i < 5; i++)
            {
                p = new PostViewModel("This is a Post " + i + "from User 12 34", null, DateTime.Now, user.DisplayedName, user.UserName.ToString(), (int)r.Next(0, 100), (int)r.Next(0, 50));
                postsList.Add(p);
            }
            var mockModel = new ProfileViewModel(user.UserName.ToString(), user.DisplayedName, user.Bio, (int)r.Next(0, 50), (int)r.Next(0, 50), postsList);

            return View(mockModel);
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
    }
}
