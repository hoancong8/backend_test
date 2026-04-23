using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.Comment
{
    public class GetCommentReplyUseCase
    {
        private readonly ICommentRepository _repo;
        public GetCommentReplyUseCase(ICommentRepository repository)
        {
            _repo = repository; 
        }
        public async Task<ApiResponse<object>> execute(string postId, int limit, string commentId)
        {
            var listComment = await _repo.GetCommentReply(postId, limit,commentId);
            return new ApiResponse<object>
            {
                success = true,
                message = "lấy bình luận bài đăng thành công!",
                data = listComment
            };
        }
    }
}