using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.src.Test.Application.Dtos;
using test.src.Test.Application.UseCases.Comment;

namespace test.src.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController
    {
        private readonly CreateCommentUseCase _createCommentUseCase;
        public CommentController(CreateCommentUseCase createCommentUseCase)
        {
            _createCommentUseCase = createCommentUseCase;
        }
       
        [HttpPost("create-comment")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentRequest req)
        {
            
            var result = await _createCommentUseCase.Execute(req);
            return new JsonResult(result);
        }

    }
}