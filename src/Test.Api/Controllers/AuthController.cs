
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
            var result = await _useCase.Execute(
                request.Email,
                request.PasswordHash,
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
            var result = await _loginUseCase.Excute(request.Email, request.Password);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            Console.WriteLine($"User ID from token: {userId}");
            var infoUser =await _getProfileUseCase.Execute(userId);
            return Ok(infoUser);
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var resultDelete = await _deleteAccountUseCase.Execute(userId);
            return Ok(resultDelete);
        }


        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UserRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var resultUpdate = await _updateProfileUseCase.execute(userId, request);
            return Ok(resultUpdate);
        }
    }
}