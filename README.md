# backend_test
Mô tả
-------
`backend_test` là một Web API đơn giản bằng ASP.NET Core (net8.0) tổ chức theo tách lớp (Domain / Application / Infrastructure / Api). Dự án sử dụng MySQL (Pomelo), JWT authentication, và Swagger để test API.
Test/
├── src/
│   ├── Test.Domain/            # 🧠 Tầng Nghiệp vụ (Core - Không phụ thuộc)
│   │   ├── Entities/           # User.cs, Project.cs, ...
│   │   ├── Exceptions/         # Custom domain exceptions
│   │   ├── Interfaces/         # IRepository, IDomainServices
│   │   ├── ValueObjects/       # Email, Money, ...
│   │   └── Common/             # BaseEntity, Audit
│
│   ├── Test.Application/       # ⚙️ Tầng Ứng dụng (Use Cases)
│   │   ├── UseCases/           # Commands & Queries (CQRS)
│   │   │   ├── Auth/           # Register, Login, v.v.
│   │   │   └── Users/          # GetUserQuery, UpdateProfile
│   │   ├── Dtos/               # Request/Response DTOs
│   │   ├── Mappings/           # AutoMapper/Mapster profiles
│   │   ├── Validators/         # FluentValidation rules
│   │   └── Behaviors/          # MediatR pipelines (Logging, Validation)
│
│   ├── Test.Infrastructure/    # 💾 Tầng Hạ tầng (Data & Services)
│   │   ├── Persistence/        # EF Core: AppDbContext, Configurations
│   │   │   ├── Configurations/ # Fluent API entity maps
│   │   │   ├── Migrations/     # EF Migrations
│   │   │   └── Repositories/   # Implementations của interfaces
│   │   ├── Services/           # EmailService, FileStorage, v.v.
│   │   └── Identity/           # JWT config, role setup
│
│   ├── Test.Api/               # 🌐 Tầng Trình diễn (Web API)
│   │   ├── Controllers/        # API endpoints (AuthController.cs)
│   │   ├── Middlewares/        # ExceptionMiddleware, Logging
│   │   ├── Configurations/     # DI extension methods
│   │   ├── Program.cs          # Host & Middleware pipeline
│   │   └── appsettings.json    # ConnectionStrings, JWT, v.v.
│
├── tests/                      # 🧪 Unit & Integration tests
│   ├── Test.UnitTests/
│   └── Test.IntegrationTests/
└── test.sln / test.csproj      # Solution / project files
Yêu cầu
--------
- .NET SDK 8.0
- `dotnet-ef` (Entity Framework Core tools)
- MySQL (hoặc MariaDB) cho môi trường dev (mặc định project dùng MySQL)

Cài đặt & chạy
----------------
1. Clone repository:

```bash
git clone <repository-url>
cd <repo-folder>
```

2. Khôi phục package:

```bash
dotnet restore
```

3. (Tuỳ chọn) cấu hình kết nối DB:

Mặc định trong `Program.cs` có chuỗi kết nối MySQL:

```csharp
var connection = "server=localhost;database=test;user=root;password=;";
```

Bạn có thể thay bằng connection string trong `appsettings.json` hoặc biến môi trường. Nếu dùng MySQL local (XAMPP), tạo database tên `test` hoặc đổi tên database cho phù hợp.

4. Tạo migration & cập nhật database:

Nếu bạn chưa có migration, tạo migration với tên `InitialCreate` và đặt output vào thư mục persistence:

```bash
dotnet ef migrations add InitialCreate --context AppDbContext --output-dir src/Test.Infrastructure/Persistence/Migrations
dotnet ef database update --context AppDbContext
```

Nếu bạn gặp lỗi, đảm bảo đang chạy lệnh từ thư mục chứa `test.csproj` (root) và `Microsoft.EntityFrameworkCore.Design` đã có trong `test.csproj`.

5. Chạy ứng dụng:

```bash
dotnet run --project test.csproj
```

Hoặc chỉ `dotnet run` nếu đang ở thư mục gốc.

Sau khi chạy, mở Swagger tại `http://localhost:5000/swagger` (mặc định port có thể khác — xem output khi `dotnet run`).

API chính & ví dụ
------------------
- `GET /WeatherForecast` — sample controller tại `Controllers/WeatherForecastController.cs`.
- `POST /api/auth/register` — đăng ký user (xem `src/Test.Api/Controllers/AuthController.cs`).
- `POST /api/auth/login` — đăng nhập, trả JWT.
- `GET /api/auth/profile` — lấy thông tin user (yêu cầu Authorization: Bearer <token>).
- `DELETE /api/auth/delete-account` — xoá account (yêu cầu Authorization).
- `PUT /api/auth/update-profile` — cập nhật profile (yêu cầu Authorization).

Ví dụ curl đăng ký và login:

```bash
# Register
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"me@example.com","passwordHash":"P@ssw0rd","firstName":"Nguyen","lastName":"A"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"me@example.com","password":"P@ssw0rd"}'

# Use token (replace <TOKEN>)
curl -H "Authorization: Bearer <TOKEN>" http://localhost:5000/api/auth/profile
```

Cấu trúc source (thực tế dự án)
--------------------------------
- `Program.cs` — khởi tạo DI, DbContext, Authentication, Swagger, Middleware (ExceptionMiddleware).
- `Controllers/WeatherForecastController.cs` — endpoint mẫu.
- `src/Test.Api/Controllers` — `AuthController.cs` (register/login/profile/...)
- `src/Test.Application/` — use-cases, DTOs, validators, behaviors.
- `src/Test.Domain/` — entities (`User`), interfaces, value objects.
- `src/Test.Infrastructure/` — `AppDbContext`, repositories, services.

Kiến trúc & hướng dẫn phát triển
--------------------------------
- Tách rõ ràng: Domain (core) không phụ thuộc vào infrastructure.
- Application layer chứa logic (UseCases) và được gọi từ Controller.
- Infrastructure chứa `AppDbContext` và `UserRepository`.
- Luôn dùng async/await cho I/O.
- Không trả Entity trực tiếp qua API; map sang DTO ở Application layer.

Xử lý lỗi
----------
Project sử dụng `ExceptionMiddleware` để chuẩn hoá lỗi. Quy ước trả lỗi:

- `ValidationException` -> 400 Bad Request
- `NotFoundException` -> 404 Not Found
- `UnauthorizedException` -> 401 Unauthorized
- `Unhandled Exception` -> 500 Internal Server Error (log chi tiết)

Các dependency quan trọng (từ `test.csproj`)
-----------------------------------------
- `Pomelo.EntityFrameworkCore.MySql` — MySQL provider cho EF Core
- `Microsoft.EntityFrameworkCore` — EF Core
- `Microsoft.AspNetCore.Authentication.JwtBearer` — JWT auth
- `Swashbuckle.AspNetCore` — Swagger
- `BCrypt.Net-Next` — password hashing

Gợi ý & bước tiếp theo
----------------------
- Muốn tôi thêm `docker-compose` (MySQL + app) không? Tôi có thể tạo file mẫu.
- Muốn tôi thêm example migration đã sẵn sàng (InitialCreate) hoặc tạo `src/Test.Infrastructure/Persistence/Migrations` mẫu không?
- Muốn tôi xuất sơ đồ cấu trúc (Mermaid) vào README hoặc xuất PNG/SVG?
