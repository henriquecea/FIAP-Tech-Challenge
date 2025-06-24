using FCG.Domain.Entity;

namespace FCG.Domain.Interface.Repository;

public interface IUserRepository : IRepository<UserEntity>
{
    /// <summary>
    /// Retrieves a user entity by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<UserEntity?> GetByEmailAsync(string email);
}
