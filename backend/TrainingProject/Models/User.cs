using System.ComponentModel.DataAnnotations;

namespace TrainingProject.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    [Required]
    public required string Role { get; set; } 

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsFirstLogin { get; set; } = true;
}

