using System;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using test.src.Test.Domain.Interfaces;
using test.Models;
using Microsoft.EntityFrameworkCore;

namespace test.src.Test.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly test.Models.TestContext _context;
        public PostRepository(test.Models.TestContext context)
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
    }
}
