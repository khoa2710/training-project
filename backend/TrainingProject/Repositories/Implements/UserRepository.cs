using Microsoft.EntityFrameworkCore;
using TrainingProject.Data;
using TrainingProject.Data.Entities;
using TrainingProject.Repositories.Abstractions;

namespace TrainingProject.Repositories.Implements;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext db) : base(db) { }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
}