namespace TrainingProject.Models.Dtos
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsFirstLogin { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}

