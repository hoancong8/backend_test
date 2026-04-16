using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test.src.Test.Application.Dtos;
using test.src.Test.Domain.Entities;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Infrastructure.Persistence;

namespace test.src.Test.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == username);
        }
        public async Task<User?> GetByUserId(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        }
        public async Task<string> DeleteByUsername(string userId)
        {
            // 1. Tìm user theo email
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            // 2. Nếu không thấy thì trả về null (không có gì để xóa)
            if (user == null) return "Không tìm thấy tài khoản để xóa";

            // 3. Đánh dấu đối tượng này là sẽ bị xóa
            _context.Users.Remove(user);

            // 4. Lưu thay đổi thực sự xuống Database
            await _context.SaveChangesAsync();

            // Trả về user vừa xóa để phía gọi hàm biết là đã xóa thành công
            return "Xóa tài khoản thành công";
        }
        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<string> UpdateProfile(string userId, UserRequest request)
        {
            // 1. Lấy dữ liệu thật từ DB
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            if (user == null) return "User không tồn tại";

            // 2. Chỉ cập nhật nếu request có giá trị (không null)
            if (!string.IsNullOrEmpty(request.FirstName)) user.FirstName = request.FirstName;
            if (!string.IsNullOrEmpty(request.LastName)) user.LastName = request.LastName;
            if (!string.IsNullOrEmpty(request.PhoneNumber)) user.PhoneNumber = request.PhoneNumber;
            if (!string.IsNullOrEmpty(request.AvatarUrl)) user.AvatarUrl = request.AvatarUrl;

            // 3. EF sẽ tự so sánh, trường nào giá trị mới giống hệt giá trị cũ 
            // thì nó cũng sẽ không đưa vào câu lệnh SQL UPDATE.
            await _context.SaveChangesAsync();

            return "Cập nhật thành công";
        }
        public Task<User?> GetProfile()
        {
            throw new NotImplementedException();
        }
    }
}