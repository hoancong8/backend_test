using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.src.Test.Application.Dtos;
using test.src.Test.Domain.Entities;

namespace test.src.Test.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetByUserId(string userId);

        Task<User?> GetProfile();
        Task<string> DeleteByUsername(string username);
        Task<string> UpdateProfile(string userId, UserRequest user);
        Task<string> CheckName(string name);
        Task Add(User user);
    }
}