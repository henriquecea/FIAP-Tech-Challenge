using FCG.Domain.Entity;

namespace FCG.Domain.Interface.Repository;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
}
