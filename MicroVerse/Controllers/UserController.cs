using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
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
            var userModel = await _context.Users.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
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

        // PATCH: api/User/id1@user.de/id2@user.com
        [HttpPatch("Follow/{followerId}/{followedId}")]
        public async Task<IActionResult> FollowUser(string followerId, string followedId)
        {
            var follows = new Follows(followerId, followedId);
            _context.Follows.Add(follows);
            await _context.SaveChangesAsync();

            return CreatedAtAction("FollowUser", new { id = followerId }, follows);
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

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
    }
}
