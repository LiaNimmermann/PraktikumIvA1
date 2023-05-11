using Microsoft.AspNetCore.Mvc;
using MicroVerse.Models;
using MicroVerse.ViewModels;
using System.Diagnostics;

namespace MicroVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEnumerable<UserModel> _users = new List<UserModel>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Creating Mock data
            var followsList = new List<UserModel>();
            var postsList = new List<PostViewModel>();
            Random r = new Random();
            UserModel f;
            PostViewModel p;
            for (int i = 0; i < 5; i++)
            {
                f = new UserModel();
                f.DisplayedName = "Followed User " + i;
                followsList.Add(f);
                p = new PostViewModel("This is a Post from User " + i, null, DateTime.Now, "Followed User " + i, "user@email.com", (int)r.Next(0, 100), (int)r.Next(0, 50));
                postsList.Add(p);
            }
            var mockModel = new HomeViewModel(new UserModel(), followsList, postsList);
            return View(mockModel);
        }
        public IActionResult Profile(string username)
        {
            //Creating Mock Data
            UserModel user = new UserModel("User1234", "User 12 34", "I'm just a normal User with a normal Bio");

            List<UserModel> followsList = new List<UserModel>();
            var postsList = new List<PostViewModel>();
            Random r = new Random();
            PostViewModel p;
            for (int i = 0; i < 5; i++)
            {
                p = new PostViewModel("This is a Post " + i + "from User 12 34", null, DateTime.Now, user.DisplayedName, user.Username.ToString(), (int)r.Next(0, 100), (int)r.Next(0, 50));
                postsList.Add(p);
            }
            var mockModel = new ProfileViewModel(user.Username.ToString(), user.DisplayedName, user.Bio, (int)r.Next(0, 50), (int)r.Next(0, 50), postsList);

            return View(mockModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("User/Create/{email}/{username}/{name}/{password}")]
        public IActionResult CreateUser(String email, String username, String name, String password)
        {
            UserModel user = new UserModel()
            {
                Email = email,
                Username = new Username() { Name = username },
                DisplayedName = name,
                Role = Role.user,
                Activation = Activation.active,
                Password = password
            };
            return Json(user);
        }

        [HttpGet("User/Delete/{email}")]
        public IActionResult DeleteUser(String email)
        {
            var user = _users.FirstOrDefault(x => email == x.Email);
            user?.Delete();
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
