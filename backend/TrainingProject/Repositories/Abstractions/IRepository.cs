using System.Linq.Expressions;

namespace TrainingProject.Repositories.Abstractions;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}