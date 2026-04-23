using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.src.Test.Application.Dtos.reponse
{
    public class CommentReponse
    {
        public string Id { get; set; }

        public string? Content { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public string? ParentCommentId { get; set; }
        public int? ReplyCount{get;set;}
        public UserDto User { get; set; }

    }
}