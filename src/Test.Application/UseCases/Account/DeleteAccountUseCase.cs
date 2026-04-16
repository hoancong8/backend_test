using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Domain.Interfaces;

namespace test.src.Test.Application.UseCases.Account
{
    public class DeleteAccountUseCase
    {
        private readonly IUserRepository _userRepository;
        public DeleteAccountUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> Execute(string userId)
        {
            var result = await _userRepository.DeleteByUsername(userId);
            return result;
        }
    }
}