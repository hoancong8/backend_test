using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Entities;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Domain.ultit;

namespace test.src.Test.Application.UseCases.Auth
{
    public class RegisterUseCase
    {
        private readonly IUserRepository _repo;

        public RegisterUseCase(IUserRepository repo)
        {
            _repo = repo;
        }
        public bool IsValidGmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            return Regex.IsMatch(email, pattern);
        }
        public async Task<ApiResponse<object>> Execute(string username, string password, string? firstName, string? lastName, string? phoneNumber, string? avatarUrl)
        {
            if (!IsValidGmail(username))
            {
                return new ApiResponse<object>
                {
                    success = false,
                    message = "Email must be a valid Gmail address",
                    data = null
                };
            }
            var existing = await _repo.GetByUsername(username);

            if (existing != null)
            { 
                return new ApiResponse<object>
                {
                    success = false,
                    message = "User already exists",    
                    data = null
                };
            }
            
            await _repo.Add(new User
            {
                Email = username,
                PasswordHash = PasswordHelper.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                AvatarUrl = avatarUrl

            });

            return new ApiResponse<object>
            {
                success = true,
                message = "Register success",
                data = null
            };
        }
    }
}