using EcommerceAI.Domain.Interfaces;

namespace EcommerceAI.Domain.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetByStatusAsync(bool isActive, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
