namespace TrainingProject.Models.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsFirstLogin { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string? Avatar { get; set; }
    public int? DepartmentId { get; set; }
}