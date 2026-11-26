using EcommerceAI.Domain.Entities;

namespace EcommerceAI.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Address>> GetAddressesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithAddressesAsync(Guid id, CancellationToken cancellationToken = default);
}
