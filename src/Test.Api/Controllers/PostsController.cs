using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.src.Test.Application.Dtos;
using test.src.Test.Application.UseCases.Post;
using test.src.Test.Application.UseCases.PostUseCase;

namespace test.src.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly CreatePostUseCase _useCase;
        private readonly RecommendPostUseCase _rcmUseCase;
        private readonly RecommendPostNotLoginUseCase _rcmNotLoginUseCase;
        public PostsController(CreatePostUseCase useCase, RecommendPostUseCase rcmUseCase,RecommendPostNotLoginUseCase rcmNotLoginUseCase)
        {
            _useCase = useCase;
            _rcmUseCase = rcmUseCase;
            _rcmNotLoginUseCase = rcmNotLoginUseCase;
        }

        [HttpPost("create-post")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreatePostRequest req)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!Guid.TryParse(userId, out var userIdGuid)) return Unauthorized();
            var result = await _useCase.Execute(userIdGuid, req);
            return Ok(result);
        }


        [HttpPost("recommend-post")]
        [Authorize]
        public async Task<IActionResult> recommend()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!Guid.TryParse(userId, out var userIdGuid)) return Unauthorized();
            var result = await _rcmUseCase.execute(userId, 10);
            return Ok(result);
        }
        [HttpPost("recommend-not-login-post")]
        
        public async Task<IActionResult> recommendNotLogin(int limit)
        {
            var result = await _rcmNotLoginUseCase.execute(10);
            return Ok(result);
        }

    }

}
