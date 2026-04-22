using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities.Models;

namespace test.src.Test.GenData
{
    public class GenProduct
    {
         public static async Task SeedProducts(TestContext context, int count = 1000)
    {
        // ❗ phải có user trước
        var userIds = await context.Users
            .Select(u => u.Id)
            .ToListAsync();

        if (!userIds.Any())
        {
            Console.WriteLine("[Seed] No users found → cannot seed products");
            return;
        }

        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.CreatedAt, f => DateTime.Now)

            // 🔥 random user
            .RuleFor(p => p.UserId, f => f.PickRandom(userIds));

        var products = faker.Generate(count);

        await context.Products.AddRangeAsync(products);

        var saved = await context.SaveChangesAsync();
        Console.WriteLine($"[Seed] Products inserted: {saved}");

        var total = await context.Products.CountAsync();
        Console.WriteLine($"[Seed] Total products: {total}");
    }
    }
}