using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.src.Test.Application.Dtos.reponse
{
    public class PostReponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }

        public UserDto User { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public List<string> Images { get; set; }
        public List<string> Videos { get; set; }
    }
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
    }
}