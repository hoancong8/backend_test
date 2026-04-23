using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.src.Test.Application.Dtos.reponse
{
    public class UserReponse
    {
        public string Id { get; set; }

        public string Email { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? AvatarUrl { get; set; }

        public string? PhoneNumber { get; set; }

        public int? Role { get; set; }
    }
}