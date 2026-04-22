using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Domain.Entities.Models;

namespace test.src.Test.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);  
        Task<Comment?> GetCommentById(string commentId);
        Task<List<Comment>> RecommendCommentsForPost(string postId, int limit);
    }
}