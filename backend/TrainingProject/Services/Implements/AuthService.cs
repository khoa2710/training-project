using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainingProject.Models.Dtos;
using TrainingProject.Repositories.Abstractions;
using TrainingProject.Services.Abstractions;

namespace TrainingProject.Services.Implements;

public class AuthService : IAuthService
{
    // Add missing IUserRepository field so constructor and methods can reference _users
    private readonly IUserRepository _users;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(request.Username, ct);
        if (user is null) return null;

        var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!ok) return null;

        // placeholder until we implement real JWT
        var token = $"DEV-TOKEN-{user.Id}-{Guid.NewGuid()}";
        // var token = GenerateJwtToken(user.Id, user.Username, user.Role);

        return new LoginResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            IsFirstLogin = user.IsFirstLogin,
            Token = token
        };
    }
    private string GenerateJwtToken(Guid userId, string username, string role)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new(JwtRegisteredClaimNames.UniqueName, username),
        new(ClaimTypes.Name, username),
        new(ClaimTypes.Role, role),
    };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}