using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Dtos.reponse;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Entities;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.Post
{
    public class RecommendPostUseCase
    {
        private readonly IPostRepository _repo;
        public RecommendPostUseCase(IPostRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<object>> execute(string userId, int limit)
        {
            var listPost = await _repo.RecommendPostsForUser(userId, limit);

            var result = listPost.Select(p => new PostReponse
            {
                Id = p.Id,
                Content = p.Content,
                CreatedAt = p.CreatedAt,

                User = new UserDto
                {
                    Id = p.User.Id.ToString(),
                    FullName = p.User.FirstName + " " + p.User.LastName,
                    AvatarUrl = p.User.AvatarUrl
                },

                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,

                Images = p.Postimages.Select(i => i.ImageUrl).ToList(),
                Videos = p.Postvideos.Select(v => v.VideoUrl).ToList()
            });

            return new ApiResponse<object>
            {
                success = true,
                message = "Get posts successfully",
                data = result
            };
        }
    }
}