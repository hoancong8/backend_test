using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.PostUseCase
{
    public class RecommendPostNotLoginUseCase
    {
        private readonly IPostRepository _repo;
        public RecommendPostNotLoginUseCase(IPostRepository repo)
        {
            _repo = repo;
        }
        public async Task<ApiResponse<object>> execute(int limit)
        {
            var result = await _repo.RecommendPosts(limit);
            return new ApiResponse<object>
            {
                success=true,
                message = "lấy bài đăng thành công",
                data = result
            };
        }
    }
}