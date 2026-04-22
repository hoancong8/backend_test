using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities.Models;

namespace test.src.Test.GenData
{
    public static class GenUser
    {
        public static async Task SeedUsers(TestContext context, int count = 1000)
        {
            // Load existing emails to avoid unique constraint violations
            var existingEmails = new HashSet<string>(await context.Users
                .Select(u => u.Email)
                .ToListAsync(), StringComparer.OrdinalIgnoreCase);

            var newUsers = new List<User>();
            var f = new Faker();

            while (newUsers.Count < count)
            {
                // build a reasonably realistic but guaranteed-unique email
                var username = f.Internet.UserName();
                var domain = f.Internet.DomainName();
                var email = $"{username}.{Guid.NewGuid().ToString().Substring(0, 8)}@{domain}";

                if (existingEmails.Contains(email)) continue;

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstName = f.Name.FirstName(),
                    LastName = f.Name.LastName(),
                    PhoneNumber = f.Phone.PhoneNumber(),
                    AvatarUrl = f.Internet.Avatar(),
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    Role = 1
                };

                newUsers.Add(user);
                existingEmails.Add(email);
            }

            Console.WriteLine($"[Seed] Prepared {newUsers.Count} new users (unique by email).");

            await context.Users.AddRangeAsync(newUsers);
            try
            {
                var saved = await context.SaveChangesAsync();
                Console.WriteLine($"[Seed] SaveChanges returned: {saved}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seed] SaveChanges exception: {ex}");
                throw;
            }

            var total = await context.Users.CountAsync();
            Console.WriteLine($"[Seed] Total users in DB after seeding: {total}");
        }
    }
}