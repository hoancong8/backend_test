using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Dtos;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Infrastructure.services;

namespace test.src.Test.Application.UseCases.Comment
{
    public class CreateCommentUseCase
    {
        private readonly ICommentRepository _repo;
        private readonly CloudinaryService _cloudinary;
        private readonly IUserRepository _userRepo;

        public CreateCommentUseCase(ICommentRepository repo, CloudinaryService cloudinary, IUserRepository userRepo)
        {
            _repo = repo;
            _cloudinary = cloudinary;
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<object>> Execute(CreateCommentRequest request)
        {
            string? imageUrl = null;
            if(request.ImageUrl != null)
            {
                imageUrl = await _cloudinary.UploadImageAsync(request.ImageUrl);
            }
            if(request.ParentCommentId != null)
            {
                var parentComment = await _repo.GetCommentById(request.ParentCommentId.ToString());
                if(parentComment == null)
                {
                    return new ApiResponse<object>
                    {
                        success = false,
                        message = "Bình luận cha không tồn tại",
                        data = null
                    };
                }
            }
            if(request.UserId == Guid.Empty)
            {
                return new ApiResponse<object>
                {
                    success = false,
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }
            if(request.UserId != Guid.Empty)
            {
                var user = await _userRepo.GetByUserId(request.UserId.ToString());
                if(user == null)
                {
                    return new ApiResponse<object>
                    {
                        success = false,
                        message = "Người dùng không tồn tại",
                        data = null
                    };
                }
            }
            var comment = new Models.Comment
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                PostId = request.PostId,
                ParentCommentId = request.ParentCommentId,
                ImageUrl = imageUrl
            };
            await _repo.AddComment(comment);
            return new ApiResponse<object>
            {
                success = true,
                message = "Bình luận đã được tạo thành công",
                data = null
            };
        }

    }
}