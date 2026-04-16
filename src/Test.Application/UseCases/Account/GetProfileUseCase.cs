using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.Auth
{
    public class GetProfileUseCase
    {
         private readonly IUserRepository _repo;
         public GetProfileUseCase(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<ApiResponse<object>> Execute(string userId)
        {
            var user = await _repo.GetByUserId(userId);
            if (user == null)
            {
                return new ApiResponse<object>
                {
                    success = false,
                    message = "User not found",
                    data = null
                };
            }
            return new ApiResponse<object>
            {
                success = true,
                message = "Profile retrieved successfully",
                data = new
                {
                    id = user.Id,
                    Email = user?.Email,
                    FirstName = user?.FirstName,
                    LastName = user?.LastName,
                    PhoneNumber = user?.PhoneNumber,
                    AvatarUrl = user?.AvatarUrl
                }
            };
        }
    }
}