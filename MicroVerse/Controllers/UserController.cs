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
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUserModel()
        {
            return await _context.UserModel.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(string id)
        {
            var userModel = await _context.UserModel.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(string id, UserModel userModel)
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
                if (!UserModelExists(id))
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
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
            _context.UserModel.Add(userModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserModelExists(userModel.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserModel", new { id = userModel.Email }, userModel);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(string id)
        {
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/User/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _context.UserModel.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Activation = Activation.blocked;

            return await PutUserModel(id, user);
        }

        // PATCH: api/User/5
        [HttpPatch("Ban/{id}")]
        public async Task<IActionResult> BanUser(string id)
        {
            var user = await _context.UserModel.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Activation = Activation.banned;

            return await PutUserModel(id, user);
        }

        // PATCH: api/User/5
        [HttpPatch("Follow/{followerId}/{followedId}")]
        public async Task<IActionResult> FollowUser(string followerId, string followedId)
        {
            var follower = await _context.UserModel.FindAsync(followerId);
            if (follower == null)
            {
                return NotFound();
            }
            var followed = await _context.UserModel.FindAsync(followedId);
            if (followed == null)
            {
                return NotFound();
            }

            follower.Follows.Add(followed);
            return await PutUserModel(followerId, follower);
        }

        [HttpGet("Follows/{id}")]
        public async Task<IActionResult> GetFollows(string id)
        {
            var user = await _context.UserModel.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Json(user.Follows);
        }

        [HttpGet("Follower/{id}")]
        public IActionResult GetFollower(string id)
        {
            var follower = _context.UserModel.Where(user => user.Follows.Any(f => f.Email == id));
            return follower == null ? NotFound() : Json(follower);
        }

        private bool UserModelExists(string id)
        {
            return _context.UserModel.Any(e => e.Email == id);
        }
    }
}
