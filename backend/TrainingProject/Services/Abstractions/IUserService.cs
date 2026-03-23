using TrainingProject.Models.Dtos;
using TrainingProject.Models.SubmitModels;

namespace TrainingProject.Services.Abstractions;

public interface IUserService : IService<UserDto, CreateUserRequest, UpdateUserRequest>
{
}