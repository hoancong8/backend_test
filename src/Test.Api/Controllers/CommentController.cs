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
        private readonly GetCommentUseCase _getCommentUseCase;
        private readonly GetCommentReplyUseCase _getCommentReplyUseCase;
        public CommentController(CreateCommentUseCase createCommentUseCase, GetCommentUseCase getCommentUseCase,GetCommentReplyUseCase getCommentReplyUseCase)
        {
            _createCommentUseCase = createCommentUseCase;
            _getCommentUseCase = getCommentUseCase;
            _getCommentReplyUseCase = getCommentReplyUseCase;
        }

        [HttpPost("create-comment")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentRequest req)
        {

            var result = await _createCommentUseCase.Execute(req);
            return new JsonResult(result);
        }
        [HttpPost("get-comment")]
        // [Authorize]
        public async Task<IActionResult> GetComment(string postId, int limit)
        {

            var result = await _getCommentUseCase.execute(postId, limit);
            return new JsonResult(result);
        }

        [HttpPost("get-comment-reply")]
        // [Authorize]
        public async Task<IActionResult> GetCommentReply(string postId, int limit,string commentId)
        {

            var result = await _getCommentReplyUseCase.execute(postId, limit,commentId);
            return new JsonResult(result);
        }


    }
}