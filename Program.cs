using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using test.src.Test.Api.Middlewares;
using test.src.Test.Application.UseCases.Account;
using test.src.Test.Application.UseCases.Auth;
using test.src.Test.Domain.Interfaces;
using test.src.Test.Domain.ultit;
using test.src.Test.Infrastructure.Persistence;

using test.src.Test.Infrastructure.Repositories;
using test.src.Test.Infrastructure.services;
using test.src.Test.Application.UseCases.Comment;
using test.src.Test.GenData;
using test.src.Test.Domain.Entities.Models;
using test.src.Test.Application.UseCases.PostUseCase;
using test.src.Test.Application.UseCases.Post;

var builder = WebApplication.CreateBuilder(args);

// 🔥 MySQL (XAMPP)
var connection = "server=localhost;database=test;user=root;password=;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// Also register TestContext (scaffolded context) so we can use it directly
builder.Services.AddDbContext<TestContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// 🔗 DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<test.src.Test.Domain.Interfaces.IPostRepository, test.src.Test.Infrastructure.Repositories.PostRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<GetProfileUseCase>();
builder.Services.AddScoped<DeleteAccountUseCase>();
builder.Services.AddScoped<UpdateProfileUseCase>();
builder.Services.AddScoped<CreatePostUseCase>();
builder.Services.AddScoped<CreateCommentUseCase>();
builder.Services.AddScoped<RecommendPostUseCase>();
builder.Services.AddScoped<GetCommentUseCase>();
builder.Services.AddScoped<GetCommentReplyUseCase>();
builder.Services.AddScoped<RecommendPostNotLoginUseCase>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CẤU HÌNH SWAGGER ĐỂ NHẬP TOKEN

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Test siêu API",
        Version = "v10000000"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var key = Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_12345678901234567890");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TestContext>();
    try
    {
        // Ensure DB exists (do not auto-seed to avoid unhandled runtime errors)
        context.Database.EnsureCreated();
        Console.WriteLine("[Seed] Database ensured.");
        await GenAll.SeedAll(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Seed] EnsureCreated failed: {ex.Message}");
    }
}
app.UseMiddleware<ExceptionMiddleware>(); 

// Lưu ý: app.UseSwagger() phải nằm trước Authentication/Authorization
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();