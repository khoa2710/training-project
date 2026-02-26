namespace TrainingProject.Services.Abstractions;

public interface IService<TDto, TCreate, TUpdate>
{
    Task<List<TDto>> GetListAsync(CancellationToken ct = default);
    Task<TDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TDto> CreateAsync(TCreate request, CancellationToken ct = default);
    Task<TDto?> UpdateAsync(int id, TUpdate request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}