using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.src.Test.GenData;
using test.Models;

namespace test.src.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly TestContext _context;
        public SeedController(TestContext context) => _context = context;

        [HttpPost("users")]
        public async Task<IActionResult> PostSeed([FromQuery] int count = 100, [FromQuery] bool force = false)
        {
            try
            {
                var existing = await _context.Users.AnyAsync();
                if (false && !force)
                {
                    var total = await _context.Users.CountAsync();
                    var sample = await _context.Users
                        .OrderBy(u => u.Email)
                        .Take(5)
                        .Select(u => new { u.Id, u.Email, u.FirstName, u.LastName })
                        .ToListAsync();

                    return Ok(new { message = "Skipped seed - users already exist", total, sample });
                }

                await GenUser.SeedUsers(_context, count);
                var totalAfter = await _context.Users.CountAsync();
                var sampleAfter = await _context.Users
                    .OrderBy(u => u.Email)
                    .Take(5)
                    .Select(u => new { u.Id, u.Email, u.FirstName, u.LastName })
                    .ToListAsync();

                return Ok(new { message = "Seed completed", total = totalAfter, sample = sampleAfter });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost("products")]
        public async Task<IActionResult> SeedProducts([FromQuery] int count = 100, [FromQuery] bool force = false)
        {
            try
            {
                await GenProduct.SeedProducts(_context, count);
                var totalAfter = await _context.Products.CountAsync();
                var sampleAfter = await _context.Products
                    .OrderBy(p => p.Name)
                    .Take(5)
                    .Select(p => new { p.Id, p.Name, p.Price })
                    .ToListAsync();

                return Ok(new { message = "Seed completed", total = totalAfter, sample = sampleAfter });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

