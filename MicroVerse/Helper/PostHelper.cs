using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    public class PostHelper
    {
        private readonly ApplicationDbContext _context;
        private List<Post> _postsCache = new List<Post>();
        private Boolean _modified = true;

        public PostHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetPosts()
            => _modified
            ? UpdateCache()
            : _postsCache;

        public IEnumerable<Post> GetPostsByUser(String userName)
            => GetPosts().Where(p => p.AuthorId == userName);

        public IEnumerable<Post> GetPostsByUserAndFollows(String userName)
        {
            var followsList = _context.Follows
                .Where(f => f.FollowingUserId == userName)
                .Select(f => f.FollowedUserId)
                .ToList();
            followsList.Add(userName);
            var posts = followsList
                .SelectMany(f => GetPostsByUser(f))
                .OrderByDescending(post => post.CreatedAt);
            return posts;
        }

        public IEnumerable<Post> GetFlaggedPosts()
            => GetPosts()
            .Where(p => p.Activation == Activation.flagged);

        public Post? GetPost(Guid id)
            => GetPosts()
            .FirstOrDefault(p => p.Id == id);

        public async Task<Status> PutPost(Guid id, Post post)
        {
            if (id != post.Id)
            {
                return Status.BadRequest;
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _modified = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return Status.NotFound;
                }
                else
                {
                    throw;
                }
            }

            return Status.NoContent;
        }

        public async Task<Post> Post
        (
            String text,
            String userName,
            Guid? postId
        )
        {
            try
            {
                var reactsTo = postId is null ? null : GetPost(postId.Value);
                var post = new Post(text, userName)
                {
                    ReactsTo = reactsTo
                };
                _context.Post.Add(post);
                await _context.SaveChangesAsync();

                return post;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _modified = true;
            }
        }

        public async Task<Status> DeletePost(Guid id)
            => await ModifyPost(id, async post =>
            {
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();

                return Status.Redirect;
            });

        public async Task<Status> BlockPost(Guid id)
            => await ModifyPostActivation(id, Activation.blocked);

        public async Task<Status> FlagPost(Guid id)
            => await ModifyPostActivation(id, Activation.flagged);

        public async Task<Status> ActivatePost(Guid id)
            => await ModifyPostActivation(id, Activation.active);

        public async Task<Status> ModifyPostActivation
        (
            Guid id,
            Activation activation
        ) => await ModifyPost(id, async post =>
        {
            post.Activation = activation;

            return await PutPost(id, post);
        });

        public async Task<Status> VotePost(Guid id, String user, Vote.Votes upvote)
            => await ModifyPost(id, async post =>
            {
                post.Votes.Add(new Vote(post.Id, user, upvote));

                return await PutPost(id, post);
            });

        public async Task<Status> ModifyPost
        (
            Guid id,
            Func<Post, Task<Status>> change
        )
        {
            try
            {
                var post = await _context.Post.FindAsync(id);
                if (post is null)
                {
                    return Status.NotFound;
                }

                return await change(post);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _modified = true;
            }
        }

        private IEnumerable<Post> UpdateCache()
        {
            _postsCache = _context.Post
                .OrderByDescending(post => post.CreatedAt)
                .ToList();
            return _postsCache;
        }

        private bool PostExists(Guid id)
        {
            return GetPosts().Any(e => e.Id == id);
        }
    }
}
