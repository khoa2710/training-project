using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingProject.Data;
using TrainingProject.Dtos;

namespace TrainingProject.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Username or password is incorrect. Please try again" });

            }

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Role = user.Role,
                IsFirstLogin = user.IsFirstLogin,
                Token = "JWT_GENERATED_LATER"
            });
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (await _context.Users.AnyAsync())
                return Ok("Already seeded.");

            _context.Users.Add(new Models.User
            {
                Username = "admin1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "Admin",
                IsFirstLogin = false,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _context.Users.Add(new Models.User
            {
                Username = "staff1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "Staff",
                IsFirstLogin = true,
                CreatedAt = DateTimeOffset.UtcNow
            });

            await _context.SaveChangesAsync();
            return Ok("Seeded: admin1/staff1 (pw: 123456)");
        }

    }
}
