using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.Helper;

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

        public UserController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => id == user.UserName);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User userModel)
        {
            if (id != userModel.Email)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userModel)
        {
            _context.Users.Add(userModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(userModel.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = userModel.Email }, userModel);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/User/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Activation = Activation.blocked;

            return await PutUser(id, user);
        }

        // PATCH: api/User/Ban/5
        [HttpPatch("Ban/{id}")]
        public async Task<IActionResult> BanUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Activation = Activation.banned;

            return await PutUser(id, user);
        }

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
            
            return new LocalRedirectResult("/Home/Profile/"+followedId);
        }

        // GET: api/User/Follows/id@user.com
        [HttpGet("Follows/{id}")]
        public IActionResult GetFollows(string id)
        {
            var users = _context.Follows
                .Where(follow => follow.FollowingUserId == id)
                .Join(
                    _context.Users,
                    follow => follow.FollowedUserId,
                    user => user.Email,
                    (follow, user) => user
                );

            return Json(users);
        }

        // GET: api/User/Follower/id@user.com
        [HttpGet("Follower/{id}")]
        public IActionResult GetFollower(string id)
        {
            var users = _context.Follows
                .Where(follow => follow.FollowedUserId == id)
                .Join(
                    _context.Users,
                    follow => follow.FollowingUserId,
                    user => user.Email,
                    (follow, user) => user
                );

            return Json(users);
        }

        [HttpGet("Follows")]
        public async Task<IActionResult> GetAllFollows()
        {
            var follows = await _context.Follows.ToListAsync();

            return Json(follows);
        }

        [HttpGet("Search/{phrase}")]
        public IActionResult SearchUsers(String phrase)
        {
            var users = (new SearchHelper(_context)).Users(phrase);

            return Json(users);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }

        [HttpPost("SetUserRole")]
        public async Task<IActionResult> SetUserRole(string userId, string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => userId == user.UserName);
            if (user == null)
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
        {
            User user = GetUser(id).Result.Value;
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Admin";
            } else if(await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                return "Moderator";
            } else
            {
                return "User";
            }
        }
    }
}
