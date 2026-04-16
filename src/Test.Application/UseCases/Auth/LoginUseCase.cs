using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Domain.ultit;

namespace test.src.Test.Application.UseCases.Auth
{

    public class LoginUseCase
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwt;
        public LoginUseCase(IUserRepository repo, JwtService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<ApiResponse<object>> Excute(string username, string password)
        {
            var user = await _repo.GetByUsername(username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                return new ApiResponse<object>
                {
                    success = false,
                    message = "Invalid username or password",
                };
            }
            var token = _jwt.GenerateToken(user.Id.ToString());

             return new ApiResponse<object>
            {
                success = true,
                message = "Login successful",
                data = new { token }
            };
        }

    }
}