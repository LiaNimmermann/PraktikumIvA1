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

namespace MicroVerse.Controllers
{
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


        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
            => Json(await _userHelper.GetUsers());

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userHelper.GetUser(id);

            return user is null
                ? NotFound()
                : user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User userModel)
            => StatusToActionResult(await _userHelper.PutUser(id, userModel));

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userModel)
            => await _userHelper.PostUser(userModel) is null
            ? Conflict()
            : CreatedAtAction("GetUser", new { id = userModel.UserName }, userModel);

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
            => StatusToActionResult(await _userHelper.DeleteUser(id));

        // PATCH: api/User/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockUser(string id)
            => StatusToActionResult(await _userHelper.BlockUser(id));

        // PATCH: api/User/Ban/5
        [HttpPatch("Ban/{id}")]
        public async Task<IActionResult> BanUser(string id)
            => StatusToActionResult(await _userHelper.BanUser(id));

        [HttpPost("FollowUser")]
        public async Task<IActionResult> FollowUser([FromForm] string followerId, [FromForm] string followedId)
        {
            var follows = new Follows(followerId, followedId);
            _context.Follows.Add(follows);
            await _context.SaveChangesAsync();

            return new LocalRedirectResult("/Home/Profile/"+followedId);
        }

        [HttpPost("UnfollowUser")]
        public async Task<IActionResult> UnfollowUser([FromForm] string followerId, [FromForm] string followedId)
        {
            var toDelete = _context.Follows.First(f => f.FollowingUserId == followerId && f.FollowedUserId == followedId);
            _context.Follows.Remove(toDelete);
            await _context.SaveChangesAsync();
            Response.Redirect(Request.HttpContext.Request.Path);

            return new LocalRedirectResult("/Home/Profile/" + followedId);
        }

        // GET: api/User/Follows/id@user.com
        [HttpGet("Follows/{id}")]
        public IActionResult GetFollows(string id)
            => Json(_followsHelper.GetFollowings(id));

        // GET: api/User/Follower/id@user.com
        [HttpGet("Follower/{id}")]
        public IActionResult GetFollower(string id)
            => Json(_followsHelper.GetFollowers(id));

        [HttpGet("Follows")]
        public async Task<IActionResult> GetAllFollows()
            => Json(_followsHelper.GetFollows());

        [HttpGet("Search/{phrase}")]
        public IActionResult SearchUsers(String phrase)
        {
            var users = (new SearchHelper(_context)).Users(phrase);

            return Json(users);
        }

        [HttpPost("SetUserRole")]
        public async Task<IActionResult> SetUserRole(string userId, string role)
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

        [HttpGet("GetUserRole")]
        public async Task<string> GetUserRole(string id)
            => await _userHelper.GetUserRole(id);

        // Log in over api, users receive a JWT for future validation
        [HttpPost]
        [Route("login")]
        //api/User/Login
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
                    expires: DateTime.Now.AddDays(21),
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

        private IActionResult StatusToActionResult(Status status)
            => status switch
            {
                Status.NoContent => NoContent(),
                Status.NotFound => NotFound(),
                _ => BadRequest()
            };
    }
}
