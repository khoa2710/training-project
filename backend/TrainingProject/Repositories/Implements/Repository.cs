using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrainingProject.Data;
using TrainingProject.Repositories.Abstractions;

namespace TrainingProject.Repositories.Implements;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public Task<List<T>> GetAllAsync(CancellationToken ct = default)
        => _set.AsNoTracking().ToListAsync(ct);

    public Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => _set.FindAsync([id], ct).AsTask();

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => _set.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

    public Task AddAsync(T entity, CancellationToken ct = default)
        => _set.AddAsync(entity, ct).AsTask();

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}