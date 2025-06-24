using FCG.Domain.Entity;
using FCG.Domain.Interface.Repository;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repository;

public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<UserEntity?> GetByEmailAsync(string email) =>
        await _dbSet.AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.EmailAddress.Address == email);
}
