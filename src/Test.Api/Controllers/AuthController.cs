
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.src.Test.Application.Dtos;
using test.src.Test.Application.UseCases.Account;
using test.src.Test.Application.UseCases.Auth;

namespace test.src.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUseCase _useCase;
        private readonly LoginUseCase _loginUseCase;
        private readonly GetProfileUseCase _getProfileUseCase;
        private readonly DeleteAccountUseCase _deleteAccountUseCase;
        private readonly UpdateProfileUseCase _updateProfileUseCase;
        public AuthController(RegisterUseCase useCase, LoginUseCase loginUseCase, GetProfileUseCase getProfileUseCase, DeleteAccountUseCase deleteAccountUseCase,UpdateProfileUseCase updateProfileUseCase)
        {
            _useCase = useCase;
            _loginUseCase = loginUseCase;
            _getProfileUseCase = getProfileUseCase;
            _deleteAccountUseCase = deleteAccountUseCase;
            _updateProfileUseCase = updateProfileUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var result = await _useCase.Execute(
                request.Email!,
                request.PasswordHash!,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.AvatarUrl
            );

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var result = await _loginUseCase.Excute(request.Email!, request.Password!);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            Console.WriteLine($"User ID from token: {userId}");
            var infoUser = await _getProfileUseCase.Execute(userId);
            return Ok(infoUser);
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var resultDelete = await _deleteAccountUseCase.Execute(userId);
            return Ok(resultDelete);
        }


        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UserRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var resultUpdate = await _updateProfileUseCase.execute(userId, request);
            return Ok(resultUpdate);
        }
    }
}