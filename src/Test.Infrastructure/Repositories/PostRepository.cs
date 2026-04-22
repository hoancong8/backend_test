using System;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using test.src.Test.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities.Models;

namespace test.src.Test.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly TestContext _context;
        public PostRepository(TestContext context)
        {
            _context = context;
        }

        public async Task CreatePost(Post post)
        {
            post.Id = post.Id == Guid.Empty ? Guid.NewGuid() : post.Id;
            post.CreatedAt = post.CreatedAt == default ? DateTime.UtcNow : post.CreatedAt;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Post>> RecommendPostsForUser(string userId, int limit)
        {
            var now = DateTime.UtcNow;
            var friendIds = await _context.Friendships
                .Where(f =>
                    (f.RequesterId.ToString() == userId || f.AddresseeId.ToString() == userId)
                    && f.Status == "Accepted")
                .Select(f => f.RequesterId.ToString() == userId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();

            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Postimages)
                .Include(p => p.Postvideos)
                .Select(p => new
                {
                    Post = p,

                    Score =
                        // 🎯 bạn bè
                        (friendIds.Contains(p.UserId) ? 100 : 0)

                        // 🎯 độ mới (càng mới càng cao)
                        + (p.CreatedAt.HasValue
                            ? 50 - EF.Functions.DateDiffHour(p.CreatedAt.Value, now)
                            : 0)

                        // 🎯 tương tác
                        + p.Likes.Count * 2
                        + p.Comments.Count * 3
                })
                .OrderByDescending(x => x.Score)
                .Take(limit)
                .Select(x => x.Post)
                .ToListAsync();
            // For simplicity, we just return the most recent posts excluding the user's own posts
            return posts;
        }
    }
}
