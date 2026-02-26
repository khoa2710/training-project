using System.Threading;
using System.Threading.Tasks;
using TrainingProject.Models.Dtos;
using TrainingProject.Models.SubmitModels;

namespace TrainingProject.Services.Abstractions;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}