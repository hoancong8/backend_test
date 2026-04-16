using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Dtos;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.Account
{
    public class UpdateProfileUseCase
    {
        private readonly IUserRepository _userRepository;
        public UpdateProfileUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> execute(string userId, UserRequest userRequest)
        {
            var result = await _userRepository.UpdateProfile(userId,userRequest);
            return result;
        }
    }
}