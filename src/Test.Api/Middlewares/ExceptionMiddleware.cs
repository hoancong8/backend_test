using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using test.src.Test.Application.Responses;

namespace test.src.Test.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho phép request đi tiếp đến Controller
                await _next(context);
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi nào xảy ra ở bất cứ đâu (Controller, UseCase, Repo)
                // Nó sẽ "văng" về đây và bị bắt lại
                _logger.LogError(ex, ex.Message); // Ghi log lỗi vào Console/File
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Lỗi 500

            // Tạo format trả về giống hệt như ApiResponse thông thường của bạn
            var response = new ApiResponse<object>
            {
                success = false,
                message = "Đã có lỗi hệ thống xảy ra",
                data = new
                {
                    error = exception.Message
                }
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}