using TrainingProject.Data.Entities;

namespace TrainingProject.Repositories.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
}   