using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.src.Test.Application.Dtos;
using test.src.Test.Domain.Interfaces;
using test.Models;
using test.src.Test.Application.UseCases.Auth;
using test.src.Test.Application.UseCases.Post;

namespace test.src.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly CreatePostUseCase _useCase;

        public PostsController(CreatePostUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("create-post")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreatePostRequest req)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!Guid.TryParse(userId, out var userIdGuid)) return Unauthorized();
            var result  = await _useCase.Execute(userIdGuid, req);
            return Ok(result);
        }
    }
}
