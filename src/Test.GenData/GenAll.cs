using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities.Models;

namespace test.src.Test.GenData
{
    public class GenAll
    {
        public static async Task SeedAll(TestContext context)
        {
            if (await context.Users.AnyAsync()) return;

            // =========================
            // 1. USERS
            // =========================
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
                .RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword("123456"));

            var users = userFaker.Generate(20);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // =========================
            // 2. TAGS
            // =========================
            var tagFaker = new Faker<Tag>()
                .RuleFor(t => t.Id, f => Guid.NewGuid())
                .RuleFor(t => t.Name, f => f.Random.Word());

            var tags = tagFaker.Generate(20);
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();

            // =========================
            // 3. POSTS
            // =========================
            var postFaker = new Faker<Post>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                .RuleFor(p => p.UserId, f => f.PickRandom(users).Id)
                .RuleFor(p => p.CreatedAt, f => f.Date.Past());

            var posts = postFaker.Generate(100);
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // =========================
            // 4. POST IMAGES & VIDEOS
            // =========================
            var images = new List<Postimage>();
            var videos = new List<Postvideo>();

            var faker = new Faker();

            foreach (var post in posts)
            {
                int imgCount = faker.Random.Int(0, 5);
                int vidCount = faker.Random.Int(0, 2);

                for (int i = 0; i < imgCount; i++)
                {
                    images.Add(new Postimage
                    {
                        Id = Guid.NewGuid(),
                        PostId = post.Id,
                        ImageUrl = faker.Image.PicsumUrl()
                    });
                }
                for (int i = 0; i < vidCount; i++)
                {
                    videos.Add(new Postvideo
                    {
                        Id = Guid.NewGuid(),
                        PostId = post.Id,
                        VideoUrl = $"https://sample-videos.com/video{i}.mp4"
                    });
                }
            }

            await context.Postimages.AddRangeAsync(images);
            await context.Postvideos.AddRangeAsync(videos);
            await context.SaveChangesAsync();

            // =========================
            // 5. COMMENTS
            // =========================
            var comments = new List<Comment>();

            foreach (var post in posts)
            {
                int count = faker.Random.Int(0, 10);

                for (int i = 0; i < count; i++)
                {
                    comments.Add(new Comment
                    {
                        Id = Guid.NewGuid(),
                        Content = faker.Lorem.Sentence(),
                        PostId = post.Id,
                        UserId = faker.PickRandom(users).Id
                    });
                }
            }

            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();

            // =========================
            // 6. LIKES (không trùng)
            // =========================
            var likes = new HashSet<(Guid, Guid)>();
            var likeList = new List<Like>();

            foreach (var post in posts)
            {
                int count = faker.Random.Int(0, users.Count);

                var randomUsers = faker.PickRandom(users, count);

                foreach (var user in randomUsers)
                {
                    if (likes.Add((user.Id, post.Id)))
                    {
                        likeList.Add(new Like
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            PostId = post.Id
                        });
                    }
                }
            }

            await context.Likes.AddRangeAsync(likeList);
            await context.SaveChangesAsync();

            // =========================
            // 7. POST TAGS
            // =========================
            foreach (var post in posts)
            {
                var randomTags = faker.PickRandom(tags, faker.Random.Int(1, 5));
                foreach (var tag in randomTags)
                {
                    post.Tags.Add(tag);
                }
            }

            await context.SaveChangesAsync();

            Console.WriteLine("🔥 SEED ALL DATA DONE");
        }
    }
}