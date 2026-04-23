using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Application.Dtos.reponse;
using test.src.Test.Domain.Entities.Models;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TestContext _context;
        public CommentRepository(TestContext context)
        {
            _context = context;
        }
        public async Task AddComment(Comment comment)
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
        public async Task<List<CommentReponse>> RecommendCommentsForPost(string postId, int limit)
        {
            var now = DateTime.UtcNow;

            var comments = await _context.Comments
                .Where(c => c.PostId == Guid.Parse(postId) && c.ParentCommentId == null)
                .OrderByDescending(c =>
                    (c.CreatedAt.HasValue
                        ? 50 - EF.Functions.DateDiffMinute(c.CreatedAt.Value, now)
                        : 0)
                    + (string.IsNullOrEmpty(c.ImageUrl) ? 0 : 10)
                )
                .Select(c => new CommentReponse
                {
                    Id = c.Id.ToString(),
                    Content = c.Content,
                    ImageUrl = c.ImageUrl,
                    CreatedAt = c.CreatedAt,
                    ReplyCount = _context.Comments.Count(x => x.ParentCommentId == c.Id),
                    User = new UserDto
                    {
                        Id = c.User.Id.ToString(),
                        FullName = c.User.FirstName + c.User.LastName,
                        AvatarUrl = c.User.AvatarUrl
                    }, // EF tự join, không cần Include
                })
                .Take(limit)
                .ToListAsync();


            return comments;
        }
        public async Task<List<CommentReponse>> GetCommentReply(string postId, int limit, string commentId)
        {
            var now = DateTime.UtcNow;

            var comments = await _context.Comments
                .Where(c => c.PostId == Guid.Parse(postId) && c.ParentCommentId.ToString() == commentId)
                .OrderByDescending(c =>
                    (c.CreatedAt.HasValue
                        ? 50 - EF.Functions.DateDiffMinute(c.CreatedAt.Value, now)
                        : 0)
                    + (string.IsNullOrEmpty(c.ImageUrl) ? 0 : 10)
                )
                .Select(c => new CommentReponse
                {
                    Id = c.Id.ToString(),
                    Content = c.Content,
                    ImageUrl = c.ImageUrl,
                    CreatedAt = c.CreatedAt,
                    ReplyCount = _context.Comments.Count(x => x.ParentCommentId == c.Id),
                    User = new UserDto
                    {
                        Id = c.User.Id.ToString(),
                        FullName = c.User.FirstName + c.User.LastName,
                        AvatarUrl = c.User.AvatarUrl
                    }, // EF tự join, không cần Include
                })
                .Take(limit)
                .ToListAsync();
            return comments;
        }
    }
}