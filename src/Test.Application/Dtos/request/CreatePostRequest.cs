using System;

namespace test.src.Test.Application.Dtos
{
    public class CreatePostRequest
    {
        public string? Content { get; set; }
        public List<IFormFile>? ImageUrls { get; set; }
        public List<IFormFile>? VideoUrls { get; set; }
    }
}
