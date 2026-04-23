using System;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using test.src.Test.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities.Models;
using test.src.Test.Application.Dtos.reponse;

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
        public async Task<List<Post>> RecommendPostsForUser(string userId, int limit, int? lastScore)
        {
            var now = DateTime.UtcNow;

            var friendIds = await _context.Friendships
                .Where(f =>
                    (f.RequesterId.ToString() == userId || f.AddresseeId.ToString() == userId)
                    && f.Status == "Accepted")
                .Select(f => f.RequesterId.ToString() == userId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();

            var query = _context.Posts
                .Select(p => new
                {
                    Post = p,

                    Score =
                        (friendIds.Contains(p.UserId) ? 100 : 0)
                        + (p.CreatedAt.HasValue
                            ? Math.Max(0, 50 - EF.Functions.DateDiffHour(p.CreatedAt.Value, now))
                            : 0)
                        + p.Likes.Count() * 2
                        + p.Comments.Count() * 3
                });

            // 🎯 cursor pagination
            if (lastScore.HasValue)
            {
                query = query.Where(x => x.Score < lastScore.Value);
            }

            var posts = await query
                .OrderByDescending(x => x.Score)
                .Take(limit)
                .Select(x => x.Post)
                .ToListAsync();

            return posts;
        }


        public async Task<List<PostReponse>> RecommendPosts(int limit)
        {
            var now = DateTime.UtcNow;
            var posts = await _context.Posts
            .Select(
                p => new PostReponse
                {
                    Id = p.Id,
                    Content = p.Content,
                    CommentCount = p.Comments.Count,
                    LikeCount = p.Likes.Count,
                    Images = p.Postimages
                        .Select(img => img.ImageUrl)
                        .ToList(),
                    Videos = p.Postvideos.Select(video => video.VideoUrl).ToList(),
                    User = new UserDto
                    {
                        Id = p.User.Id.ToString(),
                        AvatarUrl = p.User.AvatarUrl,
                        FullName = p.User.FirstName + " " + p.User.LastName
                    },
                    CreatedAt = p.CreatedAt

                }
            ).OrderByDescending(p => p.CreatedAt).Take(limit).ToListAsync();

            return posts;
        }
    }
}
