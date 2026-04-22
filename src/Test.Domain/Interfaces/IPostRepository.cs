using System;
using System.Threading.Tasks;
using System.Threading.Tasks;
using test.src.Test.Domain.Entities.Models;
namespace test.src.Test.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task CreatePost(Post post);
        Task<List<Post>> RecommendPostsForUser(string userId, int limit);
    }
}
