using TrainingProject.Data.Entities;
using TrainingProject.Models.Dtos;
using TrainingProject.Models.SubmitModels;
using TrainingProject.Repositories.Abstractions;
using TrainingProject.Services.Abstractions;

namespace TrainingProject.Services.Implements;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    public async Task<List<UserDto>> GetListAsync(CancellationToken ct = default)
    {
        var list = await _users.GetAllAsync(ct);
        return list.Select(ToDto).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(id, ct);
        return user is null ? null : ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        // example: basic rules
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required");

        var exists = await _users.GetByUsernameAsync(request.Username, ct);
        if (exists is not null)
            throw new InvalidOperationException("Username already exists");

        var entity = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            FullName = request.FullName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Bio = request.Bio,
            Avatar = request.Avatar,
            DepartmentId = request.DepartmentId
        };

        await _users.AddAsync(entity, ct);
        await _users.SaveChangesAsync(ct);

        return ToDto(entity);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var entity = await _users.GetByIdAsync(id, ct);
        if (entity is null) return null;

        if (!string.IsNullOrWhiteSpace(request.Username))
            entity.Username = request.Username;
        if (request.FullName is not null)
            entity.FullName = request.FullName;
        if (request.Email is not null)
            entity.Email = request.Email;
        if (request.DateOfBirth is not null)
            entity.DateOfBirth = request.DateOfBirth;
        if (request.PhoneNumber is not null)
            entity.PhoneNumber = request.PhoneNumber;
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            entity.IsFirstLogin = false;
        }
        if (request.Bio is not null)
            entity.Bio = request.Bio;
        if (request.Avatar is not null)
            entity.Avatar = request.Avatar;
        if (request.DepartmentId is not null)
            entity.DepartmentId = request.DepartmentId;
        if (request.Role is not null)
            entity.Role = request.Role;

        _users.Update(entity);
        await _users.SaveChangesAsync(ct);

        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _users.GetByIdAsync(id, ct);
        if (entity is null) return false;

        _users.Remove(entity);
        await _users.SaveChangesAsync(ct);
        return true;
    }

    private static UserDto ToDto(User u) => new UserDto
    {
        Id = u.Id,
        Username = u.Username,
        Role = u.Role,
        IsFirstLogin = u.IsFirstLogin,
        CreatedAt = u.CreatedAt,
        FullName = u.FullName,
        Email = u.Email,
        DateOfBirth = u.DateOfBirth,
        PhoneNumber = u.PhoneNumber,
        Bio = u.Bio,
        Avatar = u.Avatar,
        DepartmentId = u.DepartmentId
    };
}