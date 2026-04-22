using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.src.Test.Application.Dtos
{
    public class CreateCommentRequest
    {
        public string? Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}