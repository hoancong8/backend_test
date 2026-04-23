using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Dtos.reponse;
using test.src.Test.Domain.Entities.Models;
using test.src.Test.Infrastructure.Repositories;

namespace test.src.Test.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);  
        Task<Comment?> GetCommentById(string commentId);
        Task<List<CommentReponse>> RecommendCommentsForPost(string postId, int limit);
        Task<List<CommentReponse>> GetCommentReply(string postId,int limit,string commentId);
    }
}