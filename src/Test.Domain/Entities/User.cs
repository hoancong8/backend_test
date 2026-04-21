using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.src.Test.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public int Role { get; set; } = 1; // 0: admin, 1: user
    }
}