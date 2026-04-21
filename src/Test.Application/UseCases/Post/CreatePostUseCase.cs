using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Infrastructure.services;

namespace test.src.Test.Application.UseCases.Post
{
    public class CreatePostUseCase
    {
        private readonly IPostRepository _repo;
        private readonly CloudinaryService _cloudinary;

        public CreatePostUseCase(IPostRepository repo, CloudinaryService cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        public async Task<ApiResponse<object>> Execute(Guid userId, Dtos.CreatePostRequest req)
        {
            var imageUrls = new List<string>();
            var videoUrls = new List<string>();

            if (req.ImageUrls != null)
            {
                foreach (var img in req.ImageUrls)
                {
                    var url = await _cloudinary.UploadImageAsync(img);
                    imageUrls.Add(url);
                }
            }

            if (req.VideoUrls != null)
            {
                foreach (var video in req.VideoUrls)
                {
                    var url = await _cloudinary.UploadVideoAsync(video);
                    videoUrls.Add(url);
                }
            }
            var postId = Guid.NewGuid();

            var model = new Models.Post
            {
                Id = postId,
                Content = req.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,

                Postimages = imageUrls == null
                    ? new List<Postimage>()
                    : imageUrls.Select(x => new Postimage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = x,
                        PostId = postId
                    }).ToList(),

                Postvideos = videoUrls == null
                    ? new List<Postvideo>()
                    : videoUrls.Select(x => new Postvideo
                    {
                        Id = Guid.NewGuid(),
                        VideoUrl = x,
                        PostId = postId
                    }).ToList()
            };

            await _repo.CreatePost(model);
            return new ApiResponse<object>
            {
                success = true,
                message = "Post created successfully",
                data = new
                {
                    id = model.Id,
                    content = model.Content,
                    userId = model.UserId,
                    createdAt = model.CreatedAt,
                    imageUrls = model.Postimages.Select(x => x.ImageUrl).ToList(),
                    videoUrls = model.Postvideos.Select(x => x.VideoUrl).ToList()
                }
            };
        }

    }
}