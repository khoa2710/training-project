using System.ComponentModel.DataAnnotations;

namespace TrainingProject.Data.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Role { get; set; } = "Staff"; // "Admin" | "Staff"

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsFirstLogin { get; set; } = true;

    [MaxLength(255)]
    public string? FullName { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? Avatar { get; set; }

    public int? DepartmentId { get; set; }
}