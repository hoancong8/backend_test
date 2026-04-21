using System;
using System.Threading.Tasks;
using System.Threading.Tasks;
using test.Models;

namespace test.src.Test.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task CreatePost(Post post);
    }
}
