using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace test.src.Test.Infrastructure.services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        // upload image
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "posts"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }

        // upload video
        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "posts"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }
    }
}