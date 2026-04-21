using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly test.Models.TestContext _context;
        public CommentRepository(test.Models.TestContext context)
        {
            _context = context;
        }
        public async Task AddComment(test.Models.Comment comment)
        {
            comment.Id = comment.Id == Guid.Empty ? Guid.NewGuid() : comment.Id;
            comment.CreatedAt = comment.CreatedAt == default ? DateTime.UtcNow : comment.CreatedAt;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<Comment?> GetCommentById(string commentId)
        {
            return await _context.Comments.FindAsync(Guid.Parse(commentId));
        }
    }
}