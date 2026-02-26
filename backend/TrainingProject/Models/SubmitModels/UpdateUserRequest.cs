using System.ComponentModel.DataAnnotations;

namespace TrainingProject.Models.SubmitModels;

public class UpdateUserRequest
{
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 255 characters")]
    public string? Username { get; set; }

    [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string? Password { get; set; }

    [MaxLength(255, ErrorMessage = "Full name must not exceed 255 characters")]
    public string? FullName { get; set; }

    [EmailAddress(ErrorMessage = "Email must be a valid email address")]
    [MaxLength(255, ErrorMessage = "Email must not exceed 255 characters")]
    public string? Email { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [Phone(ErrorMessage = "Phone number must be valid")]
    [MaxLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [MaxLength(500, ErrorMessage = "Bio must not exceed 500 characters")]
    public string? Bio { get; set; }

    [MaxLength(500, ErrorMessage = "Avatar URL must not exceed 500 characters")]
    public string? Avatar { get; set; }

    public int? DepartmentId { get; set; }

    [StringLength(20)]
    public string? Role { get; set; }
}