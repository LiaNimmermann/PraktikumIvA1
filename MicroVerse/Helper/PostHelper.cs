using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    // The post helper manages database access for all things post related
    public class PostHelper
    {
        private readonly ApplicationDbContext _context;

        public PostHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        // get a list with all posts
        public IEnumerable<Post> GetPosts()
        {
            var posts = _context.Post
                .OrderByDescending(post => post.CreatedAt)
                .ToList();

            var votes = _context.Vote.AsEnumerable();

            foreach (var post in posts)
            {
                post.Votes = votes
                    .Where(v => v.PostId == post.Id)
                    .ToList();
            }

            return posts;
        }

        // get a certain post
        public Post? GetPost(Guid id)
        {
            var post = _context.Post.FirstOrDefault(p => p.Id == id);

            if (post is null)
            {
                return post;
            }

            var votes = _context.Vote.AsEnumerable();

            post.Votes = votes
                .Where(v => v.PostId == post.Id)
                .ToList();

            return post;
        }

        // get all posts grouped by user
        public IEnumerable<IGrouping<String, Post>> GetPostsGroupedByUser()
            => GetPosts()
            .GroupBy(post => post.AuthorId)
            .OrderByDescending(g => g.Count())
            .Take(10);

        // get all posts by a certain user
        public IEnumerable<Post> GetPostsByUser(String userName)
            => GetPosts().Where(p => p.AuthorId == userName);

        // get all posts by a certain user and by all users that that user
        // follows
        public async Task<IEnumerable<Post>> GetPostsByUserAndFollows(String userName)
        {
            var followsList = (await (new FollowsHelper(_context))
                               .GetFollowings(userName))
                .Select(u => u.UserName)
                .Append(userName);
            return GetPostsByUserAndFollows(userName, followsList);
        }

        // same as above, but the followsList should be provided as an argument
        public IEnumerable<Post> GetPostsByUserAndFollows
        (
            String userName,
            IEnumerable<String> followsList
        )
            => GetPostsGroupedByUser()
            .Where(g => followsList.Any(f => f == g.First<Post>().AuthorId))
            .SelectMany(p => p)
            .OrderByDescending(post => post.CreatedAt);

        // get all flagged posts
        public IEnumerable<Post> GetFlaggedPosts()
            => GetPosts()
            .Where(p => p.Activation == Activation.flagged);

        // change an existing post
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

        // create a new post
        public async Task<Post> Post
        (
            String text,
            String userName,
            Guid? postId
        )
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

        // delete post
        public async Task<Status> DeletePost(Guid id)
            => await ModifyPost(id, async post =>
            {
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();

                return Status.Redirect;
            });

        // block a post
        public async Task<Status> BlockPost(Guid id)
            => await ModifyPostActivation(id, Activation.blocked);

        // flag a post
        public async Task<Status> FlagPost(Guid id)
            => await ModifyPostActivation(id, Activation.flagged);

        // activate a post
        public async Task<Status> ActivatePost(Guid id)
            => await ModifyPostActivation(id, Activation.active);

        // modify the activation of a post
        public async Task<Status> ModifyPostActivation
        (
            Guid id,
            Activation activation
        ) => await ModifyPost(id, async post =>
        {
            post.Activation = activation;

            return await PutPost(id, post);
        });

        // vote on a post
        public async Task<Status> VotePost(Guid id, String user, Vote.Votes upvote)
            => await ModifyPost(id, async post =>
            {
                post.Votes.Add(new Vote(post.Id, user, upvote));

                return await PutPost(id, post);
            });

        // modify a post
        public async Task<Status> ModifyPost
        (
            Guid id,
            Func<Post, Task<Status>> change
        )
        {
            var post = await _context.Post.FindAsync(id);

            return post is null
                ? Status.NotFound
                : await change(post);
        }

        // check if a post with the given id exists
        private bool PostExists(Guid id)
        {
            return GetPosts().Any(e => e.Id == id);
        }
    }
}
