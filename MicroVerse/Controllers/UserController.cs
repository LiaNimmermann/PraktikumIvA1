using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.Helper;
using MicroVerse.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace MicroVerse.Controllers
{
    // The UserController provides an API for all things user related.
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly UserHelper _userHelper;
        private readonly FollowsHelper _followsHelper;


        public UserController
        (
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userHelper = new UserHelper(_context, _userManager);
            _followsHelper = new FollowsHelper(_context);
        }


        // get all users
        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUser()
            => Json(_userHelper.GetUsers());

        // get a user with role
        // GET: api/User/WithRole
        [HttpGet("WithRole")]
        public async Task<ActionResult<IEnumerable<UserWithRole>>> GetUserWithRole()
            => Json(_userHelper.GetUserWithRole());

        // get a user by his username
        // GET: api/User/5
        [HttpGet("{userName}")]
        public async Task<ActionResult<User>> GetUser(string userName)
        {
            var user = await _userHelper.GetUser(userName);

            return user is null
                ? NotFound()
                : user;
        }

        // Get a user by his Email
        // GET: api/User/GetuserEmail?Email=xxxxx
        [HttpGet]
        [Route("GetuserEmail")]
        public async Task<ActionResult<User>> GetUserEmail(string Email)
        {
            var user = await _userHelper.GetUserEmail(Email);

            return user is null
                ? NotFound()
                : user;
        }


        // change an existing user
        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User userModel)
            => StatusToActionResult(await _userHelper.PutUser(id, userModel));

        // change an existing user
        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userModel)
            => await _userHelper.PostUser(userModel) is null
            ? Conflict()
            : CreatedAtAction("GetUser", new { id = userModel.UserName }, userModel);

        // delete an existing user
        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
            => StatusToActionResult(await _userHelper.DeleteUser(id));

        // change a user's bio
        [Authorize]
        [HttpPost("Bio/{userName}")]
        public async Task<IActionResult> ChangeBio(String userName, [FromForm] String bio)
            => StatusToActionResult(await _userHelper.ChangeBio(userName, bio));

        // change a user's display name
        [Authorize]
        [HttpPost("DisplayName/{userName}")]
        public async Task<IActionResult> ChangeDisplayName(String userName, [FromForm] String displayName)
            => StatusToActionResult(await _userHelper.ChangeDisplayName(userName, displayName));

        // change a user's profile picture
        [Authorize]
        [HttpPost("Picture/{userName}")]
        public async Task<IActionResult> ChangeProfilePicture(String userName, [FromForm] String imgURL)
            => StatusToActionResult(await _userHelper.ChangePicture(userName, imgURL));

        // block a user
        // PATCH: api/User/5
        [Authorize(Roles = "Moderator, Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockUser(string id)
            => StatusToActionResult(await _userHelper.BlockUser(id));

        // ban a user
        // PATCH: api/User/Ban/5
        [Authorize(Roles = "Admin")]
        [HttpPatch("Ban/{id}")]
        public async Task<IActionResult> BanUser(string id)
            => StatusToActionResult(await _userHelper.BanUser(id));

        // follow a user.
        [Authorize]
        [HttpPost("FollowUser")]
        public async Task<IActionResult> FollowUser([FromForm] string followerId, [FromForm] string followedId)
        {
            await _followsHelper.FollowUser(followerId, followedId);

            return new LocalRedirectResult("/Home/Profile/"+followedId);
        }

        // unfollow a user
        [Authorize]
        [HttpPost("UnfollowUser")]
        public async Task<IActionResult> UnfollowUser([FromForm] string followerId, [FromForm] string followedId)
        {
            await _followsHelper.UnfollowUser(followerId, followedId);

            Response.Redirect(Request.HttpContext.Request.Path);

            return new LocalRedirectResult("/Home/Profile/" + followedId);
        }

        // get all users that a specific user follows
        // GET: api/User/Follows/id@user.com
        [HttpGet("Follows/{id}")]
        public async Task<IActionResult> GetFollows(string id)
            => Json(await _followsHelper.GetFollowings(id));

        // get all users that follow a specific user
        // GET: api/User/Follower/id@user.com
        [HttpGet("Follower/{id}")]
        public async Task<IActionResult> GetFollower(string id)
            => Json(await _followsHelper.GetFollowers(id));

        // get all follow relationships
        [HttpGet("Follows")]
        public async Task<IActionResult> GetAllFollows()
            => Json(await _followsHelper.GetFollows());

        // search for a user
        [HttpGet("Search/{phrase}")]
        public IActionResult SearchUsers(String phrase)
        {
            var users = (new SearchHelper(_context)).Users(phrase);

            return Json(users);
        }

        // set a user's role
        [HttpPost("SetUserRole")]
        public async Task<IActionResult> SetUserRole([FromForm] string userId, [FromForm] string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => userId == user.UserName);
            if (user is null)
            {
                return NotFound();
            }
            switch (role)
            {
                case "Admin":
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _userManager.AddToRoleAsync(user, "Moderator");
                    break;
                case "Moderator":
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                    await _userManager.AddToRoleAsync(user, "Moderator");
                    break;
                case "User":
                    await _userManager.RemoveFromRolesAsync(user, new List<string> {"Admin", "Moderator"});
                    break;
                default:
                    return BadRequest();
            }
            return NoContent();
        }

        // get a user's role
        [HttpGet("GetUserRole")]
        public async Task<string> GetUserRole(string id)
            => await _userHelper.GetUserRole(id);

        // Log in over api, users receive a JWT for future validation
        [HttpPost]
        [Route("login")]
        //POST: api/User/Login
        public async Task<ActionResult> LogIn([FromBody] LogInViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authclaims = new[]
                {
                    new Claim("Email", model.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddDays(3),
                    claims: authclaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    tokenExpiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        // a helper method that converts a Status (returned by UserHelper
        // methods) to an ActionResult
        private IActionResult StatusToActionResult(Status status)
            => status switch
            {
                Status.NoContent => NoContent(),
                Status.NotFound => NotFound(),
                _ => BadRequest()
            };
    }
}
